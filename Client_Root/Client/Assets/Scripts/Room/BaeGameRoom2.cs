using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaeGameRoom2 : IGameRoom
{
    [SerializeField] private InputManager           m_InputManager = null;
    [SerializeField] private Camera                 m_CameraMain = null;
    [SerializeField] private CameraController       m_CameraController = null;
    [SerializeField] private SkillController        m_SkillController = null;
    [SerializeField] private UICountTimer           m_UICountTimer = null;
    [SerializeField] private GameObject             m_goGreyCover = null;
    [SerializeField] private DirectionKey           m_RotationController = null;
    [SerializeField] private UILabel                m_lbHP = null;
    [SerializeField] private UILabel                m_lbMP = null;
    [SerializeField] private UILabel                m_lbMovePoint = null;
    [SerializeField] private UILabel                m_Dummy = null;

    public static new BaeGameRoom2 Instance
    {
        get
        {
            return (BaeGameRoom2)instance;
        }
    }

    private int m_nOldFrameRate = 0;
    private int m_nUserPlayerIndex = -1;
    private int m_nUserEntityID = -1;
    private float m_fElapsedTime = 0;       //  not used..

    private float m_fTickInterval = 0.025f;
    private int m_nTick = 0;
    private int m_nServerTick = -1;
    private Dictionary<int, List<IPlayerInput>> m_dicPlayerInput = new Dictionary<int, List<IPlayerInput>>();
    private Dictionary<int, IEntity> m_dicEntity = new Dictionary<int, IEntity>();

    private Dictionary<int, int> m_dicPlayerEntity = new Dictionary<int, int>();      //  key : PlayerIndex, value : EntityID
    private Dictionary<int, int> m_dicEntityPlayer = new Dictionary<int, int>();      //  key : EntityID, value : PlayerIndex

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
        Application.targetFrameRate = 30;

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

        Application.targetFrameRate = m_nOldFrameRate;
    }
#endregion

#region Game Logic
    private IEnumerator Loop()
    {
        while (true)
        {
            while(m_nTick > m_nServerTick)
            {
                yield return null;
            }

            int nCountToProcess = 1;
            if (m_nServerTick - m_nTick >= 3)
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

                m_nTick++;
            }

            yield return null;
        }
    }

    private void Draw()
    {
        foreach (IEntity entity in m_dicEntity.Values)
        {
            entity.Draw(m_nTick);
        }
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

                    if (!character.IsAlive())
                        continue;

                    character.GetBehavior(BehaviorID.ROTATION).Start(m_fTickInterval, m_nTick, rotation.m_vec3Rotation);
                    character.GetBehavior(BehaviorID.MOVE).Start(m_fTickInterval, m_nTick);
                }
                else if (input.GetPlayerInputType() == FBS.PlayerInputType.Position)
                {
                    PlayerInput.Position position = input as PlayerInput.Position;

                    Character character = m_dicEntity[position.m_nEntityID] as Character;

                    if (!character.IsAlive())
                        continue;

                    character.GetBehavior(BehaviorID.JUMP).Start(m_fTickInterval, m_nTick);
                }
            }
        }
    }

    private void UpdateWorld()
    {
        foreach(IEntity entity in m_dicEntity.Values)
        {
            entity.UpdateStates(m_nTick);
        }

        foreach(IEntity entity in m_dicEntity.Values)
        {
            entity.UpdateBehaviors(m_nTick);
        }
    }

    private void LateUpdateWorld()
    {
        foreach(IEntity entity in m_dicEntity.Values)
        {
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
    }

    private void StartGame()
    {
        m_InputManager.Work(100, 500/*temp.. always 200, 200*/, m_CameraMain, OnClicked);
        m_RotationController.onHold += OnRotationControllerHold;

        m_nServerTick = 0;

        StartCoroutine(Loop());
    }

    private IEnumerator PrepareGame()
    {
        //  prefare for game
        yield return SceneManager.LoadSceneAsync("BasicStraightWay"/*temp.. always TestMap*/, LoadSceneMode.Additive);

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

    public float GetTickInterval()
    {
        return m_fTickInterval;
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

        ObjectPool.Instance.ReturnObject(iMsg);
    }

    private void OnEnterRoomToC(EnterRoomToC msg)
    {
        if (msg.m_nResult == 0)
        {
            m_nUserPlayerIndex = msg.m_nUserPlayerIndex;

            foreach (FBS.PlayerInfo player in msg.m_listPlayerInfo)
            {
                Character character = Factory.Instance.CreateCharacter(player.EntityID, player.MasterDataID, Character.Role.Challenger);
                character.InitStatus(new CharacterStatus(player.Status));

                m_dicEntity[player.EntityID] = character;
                m_dicPlayerEntity[player.PlayerIndex] = player.EntityID;
                m_dicEntityPlayer[player.EntityID] = player.PlayerIndex;

                if (player.PlayerIndex == m_nUserPlayerIndex)
                {
                    m_nUserEntityID = player.EntityID;

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
#endregion
}