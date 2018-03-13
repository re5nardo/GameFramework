using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaeGameRoom2 : IGameRoom
{
    [SerializeField] private InputManager           m_InputManager = null;
    [SerializeField] private GameItemManager        m_GameItemManager = null;
    [SerializeField] private Camera                 m_CameraMain = null;
    [SerializeField] private CameraController       m_CameraController = null;
    [SerializeField] private SkillController        m_SkillController = null;
    [SerializeField] private GameItemButton[]       m_GameItemButtons = null;
    [SerializeField] private UICountTimer           m_UICountTimer = null;
    [SerializeField] private GameObject             m_goGreyCover = null;
    [SerializeField] private DirectionKey           m_RotationController = null;
    [SerializeField] private UILabel                m_lbHP = null;
    [SerializeField] private UILabel                m_lbMP = null;
    [SerializeField] private UILabel                m_lbMovePoint = null;
    [SerializeField] private UILabel                m_lbNotice = null;
    [SerializeField] private UILabel                m_lbInfo = null;
    [SerializeField] private UILabel                m_lbTime = null;

    public static new BaeGameRoom2 Instance
    {
        get
        {
            return (BaeGameRoom2)instance;
        }
    }

    private const float TOUCH_PERCEPTION_TIME = 0.2f;

    private int m_nOldFrameRate = 0;
    private int m_nUserPlayerIndex = -1;
    private int m_nUserEntityID = -1;
    private float m_fElapsedTime = 0;       //  not used..
    private int m_nEntitySequence = 0;

    private float m_fTickInterval = 0.025f;
    private int m_nTick = 0;
    private int m_nEndTick = 0;
    private int m_nServerTick = -1;
    private Dictionary<int, List<IPlayerInput>> m_dicPlayerInput = new Dictionary<int, List<IPlayerInput>>();
    private Dictionary<int, IEntity> m_dicEntity = new Dictionary<int, IEntity>();

    private Dictionary<int, int> m_dicPlayerEntity = new Dictionary<int, int>();      //  key : PlayerIndex, value : EntityID
    private Dictionary<int, int> m_dicEntityPlayer = new Dictionary<int, int>();      //  key : EntityID, value : PlayerIndex
    private List<MovingObject> m_listMovingObject = new List<MovingObject>();
    private List<ICharacterAI> m_listDisturber = new List<ICharacterAI>();

    private List<PlayerRankInfo> m_listPlayerRankInfo = new List<PlayerRankInfo>();

#region Room Logic
    private void Update()
    {
        if (GetUserCharacter() == null || !GetUserCharacter().IsAlive())
            return;

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            OnRotationControllerHold(new Vector2(-1, -1));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            OnRotationControllerHold(new Vector2(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            OnRotationControllerHold(new Vector2(1, -1));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            OnRotationControllerHold(new Vector2(-1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            OnRotationControllerHold(new Vector2(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            OnRotationControllerHold(new Vector2(-1, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            OnRotationControllerHold(new Vector2(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            OnRotationControllerHold(new Vector2(1, 1));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnClicked(Vector3.zero);
        }
    }

    private void Start()
    {
        m_nOldFrameRate = Application.targetFrameRate;
//        Application.targetFrameRate = 30;

        m_RotationController.onHold += OnRotationControllerHold;

        foreach(GameItemButton gameItemButton in m_GameItemButtons)
        {
            gameItemButton.onClicked = OnGameItemButtonClicked;
        }

        RoomNetwork.Instance.ConnectToServer(Config.Instance.GetRoomServerIP(), Config.Instance.GetRoomServerPort(), OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bResult)
    {
        Debug.Log("OnConnected! Result : " + bResult);

        if (!bResult)
        {
            Debug.LogError("OnConnected Fail!");
            SceneManager.LoadScene("Lobby");

            return;
        }

        EnterRoomToR msgToR = new EnterRoomToR ();
        msgToR.m_strPlayerKey = SystemInfo.deviceUniqueIdentifier;
        msgToR.m_nAuthKey = 0;
        msgToR.m_nMatchID = 0;

        RoomNetwork.Instance.Send(msgToR);
    }

    private void OnDestroy()
    {
        if (RoomNetwork.GetInstance() != null)
        {
            RoomNetwork.Instance.RemoveConnectHandler(OnConnected);
            RoomNetwork.Instance.RemoveRecvMessageHandler(OnRecvMessage);
        }

        m_RotationController.onHold -= OnRotationControllerHold;

        foreach(GameItemButton gameItemButton in m_GameItemButtons)
        {
            gameItemButton.onClicked = null;
        }

        Application.targetFrameRate = m_nOldFrameRate;
    }
#endregion

#region Game Logic
    //  should save client.version to meta file ( for checking compatibility)
    private IEnumerator Loop()
    {
        for (int i = 0; i < 100; ++i)
        {
            MovingObject movingObject = ObjectPool.Instance.GetGameObject("MovingObject").GetComponent<MovingObject>();

            movingObject.Initialize();
            m_listMovingObject.Add(movingObject);
        }

        foreach(MovingObject mObj in m_listMovingObject)
        {
            mObj.StartTick(0);
        }

        foreach(ICharacterAI disturber in m_listDisturber)
        {
            disturber.Initialize(4, m_fTickInterval);
            disturber.StartTick(0);
        }

        yield return new WaitForSeconds(TOUCH_PERCEPTION_TIME); //  Fill buffer

        while (true)
        {
            while(m_nTick > m_nServerTick)  //  정상적인 상황 X (네트워크 지연 또는 순단 등등에 의해 버퍼가 비어버린 상황)
            {
                yield return new WaitForSeconds(TOUCH_PERCEPTION_TIME); //  Fill buffer
            }

            int nCountToProcess = 1;
            if (m_nServerTick - m_nTick >= 40)
            {
                nCountToProcess = 10;
            }
            else if (m_nServerTick - m_nTick >= 20)
            {
                nCountToProcess = 4;
            }
            else if (m_nServerTick - m_nTick >= 3)
            {
                nCountToProcess = 2;
            }

            for (int i = 0; i < nCountToProcess; ++i)
            {
                ProcessInput();

                UpdateWorld();
                LateUpdateWorld();

                //  optional?
                Draw();

                Physics.Simulate(m_fTickInterval);

                if (m_nTick == m_nEndTick)
                {
                    GameResultToR resultToR = ObjectPool.Instance.GetObject<GameResultToR>();
                    resultToR.m_listPlayerRankInfo = GetPlayerRankInfos();

                    RoomNetwork.Instance.Send(resultToR);

                    yield break;
                }

                m_nTick++;
            }

            yield return new WaitForSeconds(m_fTickInterval);
        }
    }

    private List<PlayerRankInfo> GetPlayerRankInfos()
    {
        List<Character> listRetire = new List<Character>();
        foreach(KeyValuePair<int, int> kv in m_dicPlayerEntity)
        {
            int nPlayerIndex = kv.Key;
            int nEntityID = kv.Value;

            if (m_listPlayerRankInfo.Exists(x => x.m_nPlayerIndex == nPlayerIndex))
                continue;

            Character character = m_dicEntity[nEntityID] as Character;
            listRetire.Add(character);
        }

        listRetire.Sort(CompareCharacterHeight);

        int nRank = m_listPlayerRankInfo.Count + 1;
        float fHeight = listRetire[0].GetCurrentHeight();

        for (int i = 0; i < listRetire.Count; ++i)
        {
            Character character = listRetire[i];
            if (character.GetCurrentHeight() != fHeight)
            {
                nRank++;
                fHeight = character.GetCurrentHeight();
            }

            PlayerRankInfo info = new PlayerRankInfo();
            info.m_nPlayerIndex = m_dicEntityPlayer[character.GetID()];
            info.m_nRank = nRank;
            info.m_fHeight = fHeight;

            m_listPlayerRankInfo.Add(info);
        }

        return m_listPlayerRankInfo;
    }

    private int CompareCharacterHeight(Character a, Character b)
    {
        return a.GetCurrentHeight().CompareTo(b.GetCurrentHeight());
    }

    private float GetUserRank()
    {
        float fUserHeight = GetUserCharacter().GetCurrentHeight();
        int nRank = 1;

        foreach(int nID in m_dicPlayerEntity.Values)
        {
            Character character = m_dicEntity[nID] as Character;

            if (character.GetID() == GetUserEntityID())
                continue;

            if (character.GetCurrentHeight() > fUserHeight)
                nRank++;
        }

        return nRank;
    }

    private void Draw()
    {
        foreach (IEntity entity in m_dicEntity.Values)
        {
            entity.Draw(m_nTick);
        }

        m_lbInfo.text = string.Format("{0} / {1}\n현재 높이 : {2}m\n최고 높이 : {3}m", GetUserRank(), m_dicPlayerEntity.Count, (int)GetUserCharacter().GetCurrentHeight(), (int)GetUserCharacter().GetBestHeight());

        SetRemainTime((m_nEndTick - m_nTick) * m_fTickInterval);
    }

    private void ProcessInput()
    {
        if (m_dicPlayerInput.ContainsKey(m_nTick))
        {
            foreach (IPlayerInput input in m_dicPlayerInput[m_nTick])
            {
                if (input.GetPlayerInputType() == FBS.PlayerInputType.Rotation)
                {
                    PlayerInput.Rotation rotation = input as PlayerInput.Rotation;

                    Character character = m_dicEntity[rotation.m_nEntityID] as Character;

                    if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Faint))
                        continue;

                    character.GetBehavior(BehaviorID.ROTATION).StartTick(m_nTick, rotation.m_vec3Rotation);
                    character.GetBehavior(BehaviorID.MOVE).StartTick(m_nTick);
                }
                else if (input.GetPlayerInputType() == FBS.PlayerInputType.Position)
                {
                    PlayerInput.Position position = input as PlayerInput.Position;

                    Character character = m_dicEntity[position.m_nEntityID] as Character;

                    if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Faint))
                        continue;

                    character.GetBehavior(BehaviorID.JUMP).StartTick(m_nTick);
                }
                else if (input.GetPlayerInputType() == FBS.PlayerInputType.GameItem)
                {
                    PlayerInput.GameItem gameItem = input as PlayerInput.GameItem;

                    Character character = m_dicEntity[m_dicPlayerEntity[gameItem.m_nPlayerIndex]] as Character;

                    if (!character.IsAlive())
                        continue;
                    
                    character.OnUseGameItem(gameItem.m_nGameItemID);
                }
            }
        }
    }

    private void UpdateWorld()
    {
        foreach(MovingObject mObj in m_listMovingObject)
        {
            mObj.UpdateTick(m_nTick); 
        }

        foreach(ICharacterAI disturber in m_listDisturber)
        {
            disturber.UpdateTick(m_nTick);
        }

        //  Copy values because m_dicEntity can be modified during iterating
        IEntity[] entities = new IEntity[m_dicEntity.Values.Count];
        m_dicEntity.Values.CopyTo(entities, 0);
        foreach(IEntity entity in entities)
        {
            //  !entity.isalive() return;

            entity.UpdateStates(m_nTick);
        }

        entities = new IEntity[m_dicEntity.Values.Count];
        m_dicEntity.Values.CopyTo(entities, 0);
        foreach(IEntity entity in entities)
        {
            //  !entity.isalive() return;

            entity.UpdateBehaviors(m_nTick);
        }

        m_GameItemManager.UpdateTick(m_nTick);
    }

    private void LateUpdateWorld()
    {
        //  Copy values because m_dicEntity can be modified during iterating
        IEntity[] entities = new IEntity[m_dicEntity.Values.Count];
        m_dicEntity.Values.CopyTo(entities, 0);
        foreach(IEntity entity in entities)
        {
            //  !entity.isalive() return;

            entity.LateUpdateWorld(m_nTick);
        }
    }

    private void ResetGame()
    {
        m_nUserPlayerIndex = -1;
        m_nUserEntityID = -1;
        m_fElapsedTime = 0;

        m_fTickInterval = 0.025f;
        m_nTick = 0;
        m_nServerTick = -1;
        m_dicPlayerInput.Clear();
        m_dicEntity.Clear();
        m_dicPlayerEntity.Clear();
        m_dicEntityPlayer.Clear();
        m_listMovingObject.Clear();
        m_listDisturber.Clear();

        m_listPlayerRankInfo.Clear();
    }

    private void SetDisturber()
    {

//        BlobAI blobAI = new BlobAI();
//        blobAI.Initialize(4);
//
//        m_listDisturber.Add(blobAI);
    }

    private void StartGame()
    {
        StartCoroutine(Loop());
    }

    private IEnumerator PrepareGame()
    {
        //  prefare for game
        yield return SceneManager.LoadSceneAsync("TestMap3"/*temp.. always TestMap*/, LoadSceneMode.Additive);

        m_InputManager.Work(100, 500/*temp.. always 200, 200*/, m_CameraMain, OnClicked);

        SetDisturber();

        PreparationStateToR preparationStateToR = new PreparationStateToR();
        preparationStateToR.m_fState = 1.0f;

        RoomNetwork.Instance.Send(preparationStateToR);
    }

    public override float GetElapsedTime()
    {
        return m_fElapsedTime;
    }

    public override int GetUserPlayerIndex()
    {
        return m_nUserPlayerIndex;
    }

    public int GetUserEntityID()
    {
        return m_nUserEntityID;
    }

    public Character GetUserCharacter()
    {
        if (!m_dicEntity.ContainsKey(m_nUserEntityID))
            return null;

        return (Character)m_dicEntity[m_nUserEntityID];
    }

    public void AddCharacterStatusChangeGameEvent(float fEventTime, int nEntityID, string strStatusField, string strReason, float fValue)
    {

    }

    public int GetCurrentTick()
    {
        return m_nTick;
    }

    public float GetTickInterval()
    {
        return m_fTickInterval;
    }

    public void CreateCharacter(int nMasterDataID, ref int nEntityID, ref Character character, Character.Role role, CharacterStatus status)
    {
        nEntityID = m_nEntitySequence++;

        character = Factory.Instance.CreateCharacter(nMasterDataID);
        character.Initialize(nEntityID, nMasterDataID, role);
        character.InitStatus(status);

        m_dicEntity[nEntityID] = character;
    }

    public void CreateProjectile(int nMasterDataID, ref int nEntityID, ref Projectile projectile, int nCreatorID)
    {
        nEntityID = m_nEntitySequence++;

        projectile = Factory.Instance.CreateProjectile(nMasterDataID);
        projectile.Initialize(nEntityID, nMasterDataID);

        m_dicEntity[nEntityID] = projectile;
    }

    public void GetPlayersHeight(ref float fTop, ref float fBottom)
    {
        float fTempTop = float.MinValue;
        float fTempBottom = float.MaxValue;

        foreach(int nEntityID in m_dicPlayerEntity.Values)
        {
            IEntity player = m_dicEntity[nEntityID];

            float fHeight = player.GetPosition().y;

            fTempTop = Mathf.Max(fTempTop, fHeight);
            fTempBottom = Mathf.Min(fTempBottom, fHeight);
        }

        fTop = fTempTop;
        fBottom = fTempBottom;
    }

    private void SetRemainTime(float fRemainTime)
    {
        int sec = (int)fRemainTime;
        int milliSec = (int)((fRemainTime - sec) * 1000);

        m_lbTime.text = string.Format("{0}:{1:000}", sec, milliSec);
    }
#endregion

#region Network Message Handler
    private void OnRecvMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == EnterRoomToC.MESSAGE_ID)
        {
            OnEnterRoomToC((EnterRoomToC)iMsg);
        }
        else if (iMsg.GetID() == GameStartToC.MESSAGE_ID)
        {
            OnGameStartToC((GameStartToC)iMsg);
        }
        else if (iMsg.GetID() == TickInfoToC.MESSAGE_ID)
        {
            OnTickInfoToC((TickInfoToC)iMsg);
        }
        else if (iMsg.GetID() == GameEndToC.MESSAGE_ID)
        {
            OnGameEndToC((GameEndToC)iMsg);
        }

        ObjectPool.Instance.ReturnObject(iMsg);
    }

    private void OnEnterRoomToC(EnterRoomToC msg)
    {
        if (msg.m_nResult == 0)
        {
            m_nUserPlayerIndex = msg.m_nUserPlayerIndex;

            foreach (FBS.PlayerInfo player in msg.m_listPlayerInfo)
            {
                int nEntityID = 0;
                Character character = null;
                CreateCharacter(player.MasterDataID, ref nEntityID, ref character, Character.Role.Challenger, new CharacterStatus(player.Status));

                m_dicPlayerEntity[player.PlayerIndex] = nEntityID;
                m_dicEntityPlayer[nEntityID] = player.PlayerIndex;

                if (player.PlayerIndex == m_nUserPlayerIndex)
                {
                    m_nUserEntityID = nEntityID;

                    m_CameraController.SetTarget(character.GetModelTransform());
                    m_CameraController.StartFollowTarget();

                    //m_SkillController.SetSkills(new List<int>(){0, 1, 2});

                    m_lbHP.text = string.Format("x{0}", player.Status.HP);
                    m_lbMP.text = string.Format("x{0}", player.Status.MP);
                    m_lbMovePoint.text = string.Format("x{0}", player.Status.MovePoint);
                }
            }

            StartCoroutine(PrepareGame());
        }
        else
        {
            Debug.LogError("Enter Room Fail! m_nResult : " + msg.m_nResult);
            SceneManager.LoadScene("Lobby");
        }
    }

    private void OnGameStartToC(GameStartToC msg)
    {
//        ResetGame();

        m_fTickInterval = msg.m_fTickInterval;
        Random.InitState(msg.m_nRandomSeed);
        m_nEndTick = (int)(msg.m_nTimeLimit / m_fTickInterval) - 1;

        SetRemainTime(msg.m_nTimeLimit);

        StartGame();
    }

    private void OnTickInfoToC(TickInfoToC msg)
    {
        if (msg.m_listPlayerInput.Count > 0)
        {
            if (!m_dicPlayerInput.ContainsKey(msg.m_nTick))
            {
                m_dicPlayerInput[msg.m_nTick] = new List<IPlayerInput>();
            }

            m_dicPlayerInput[msg.m_nTick].AddRange(msg.m_listPlayerInput);
        }

        m_nServerTick = msg.m_nTick;
    }

    private void OnGameEndToC(GameEndToC msg)
    {
        Debug.LogWarning("GameEndToC!");

        GameResultPopup popup = PopupManager.Instance.ShowPopup("GameResultPopup") as GameResultPopup;
        popup.SetData(msg.m_listPlayerRankInfo);
    }
#endregion

#region Event Handler
    private void OnClicked(Vector3 vec3Pos)
    {
        if (!GetUserCharacter().IsAlive())
            return;
        
        PlayerInputToR inputToR = ObjectPool.Instance.GetObject<PlayerInputToR>();
        inputToR.m_Type = FBS.PlayerInputType.Position;

        PlayerInput.Position position = ObjectPool.Instance.GetObject<PlayerInput.Position>();
        position.m_nEntityID = m_nUserEntityID;
        position.m_vec3Position = vec3Pos;

        inputToR.m_Data = position.Serialize();

        RoomNetwork.Instance.Send(inputToR);
    }

    private void OnRotationControllerHold(Vector2 vec2Direction)
    {
        if (vec2Direction == Vector2.zero)
        {
            Debug.LogWarning("[OnRotationControllerHold] vec2Direction is zero!");
            return;
        }

        if (!GetUserCharacter().IsAlive())
            return;

        float y = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(vec2Direction.x, 0, vec2Direction.y));

        if (vec2Direction.x < 0)
        {
            y = 360 - y;
        }

        PlayerInputToR inputToR = ObjectPool.Instance.GetObject<PlayerInputToR>();
        inputToR.m_Type = FBS.PlayerInputType.Rotation;

        PlayerInput.Rotation rotation = ObjectPool.Instance.GetObject<PlayerInput.Rotation>();
        rotation.m_nEntityID = m_nUserEntityID;
        rotation.m_vec3Rotation = new Vector3(0, y, 0);

        inputToR.m_Data = rotation.Serialize();

        RoomNetwork.Instance.Send(inputToR);
    }

    private void OnGameItemButtonClicked(GameItem gameItem)
    {
        if (!GetUserCharacter().IsAlive())
            return;

        PlayerInputToR inputToR = ObjectPool.Instance.GetObject<PlayerInputToR>();
        inputToR.m_Type = FBS.PlayerInputType.GameItem;

        PlayerInput.GameItem inputGameItem = ObjectPool.Instance.GetObject<PlayerInput.GameItem>();
        inputGameItem.m_nPlayerIndex = m_nUserPlayerIndex;
        inputGameItem.m_nGameItemID = gameItem.GetID();

        inputToR.m_Data = inputGameItem.Serialize();

        RoomNetwork.Instance.Send(inputToR);
    }

    public void OnJumpButtonClicked()
    {
        if (!GetUserCharacter().IsAlive())
            return;

        PlayerInputToR inputToR = ObjectPool.Instance.GetObject<PlayerInputToR>();
        inputToR.m_Type = FBS.PlayerInputType.Position;

        PlayerInput.Position position = ObjectPool.Instance.GetObject<PlayerInput.Position>();
        position.m_nEntityID = m_nUserEntityID;
        position.m_vec3Position = Vector3.zero;

        inputToR.m_Data = position.Serialize();

        RoomNetwork.Instance.Send(inputToR);
    }
#endregion

#region Game Event Handler
    public void OnPlayerDie(int nKilledEntityID, int nKillerEntityID)
    {
        Debug.Log(string.Format("{0}이 {1}을 처치했습니다.", nKillerEntityID, nKilledEntityID));

        if (nKilledEntityID == m_nUserEntityID)
        {
            MasterData.Behavior behaviorMasterData = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(BehaviorID.DIE, ref behaviorMasterData);

            //  Die Effect On
            m_goGreyCover.SetActive(true);
            m_UICountTimer.Work(behaviorMasterData.m_fLength);
            m_UICountTimer.Show();
        }
    }

    public void OnUserStatusChanged(CharacterStatus status)
    {
        m_lbHP.text = string.Format("x{0}", status.m_nHP);
        m_lbMP.text = string.Format("x{0}", status.m_nMP);
        m_lbMovePoint.text = string.Format("x{0}", (int)status.m_fMovePoint);
    }

    public void OnUserGameItemChanged(List<GameItem> listGameItem)
    {
        for (int i = 0; i < m_GameItemButtons.Length; ++i)
        {
            if (listGameItem.Count > i)
            {
                m_GameItemButtons[i].SetData(listGameItem[i]);
            }
            else
            {
                m_GameItemButtons[i].SetData(null);
            }
        }
    }
#endregion
}