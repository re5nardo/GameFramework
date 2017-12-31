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

    public static float deltaTime = 0;

    private Dictionary<int, Entity> m_dicEntity = new Dictionary<int, Entity>();

    private int m_nOldFrameRate = 0;
    private int m_nUserPlayerIndex = -1;
    private int m_nUserEntityID = -1;

    private float m_fLastUpdateTime = -0.001f;  //  include (The reason why default value is -0.001 is for processing events that happen at 0 sec)
    private float m_fElapsedTime = 0;           //  include
    private float m_fLastWorldInfoTime = -1;
    private bool m_bEntitySample = false;
    private Dictionary<int, List<IGameEvent>> m_dicGameEvent = new Dictionary<int, List<IGameEvent>>();
    private Dictionary<int, HashSet<IGameEvent>> m_dicProcessedGameEvent = new Dictionary<int, HashSet<IGameEvent>>();


    private float m_fTickInterval = 0.1f;


    private void Update()
    {
        if (GetUserCharacter() == null || !GetUserCharacter().IsAlive())
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            


            GameInputMoveToR moveToR = ObjectPool.Instance.GetObject<GameInputMoveToR>();
            moveToR.m_nPlayerIndex = m_nUserPlayerIndex;
            moveToR.m_Direction = FBS.MoveDirection.Up;

            RoomNetwork.Instance.Send(moveToR);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameInputMoveToR moveToR = ObjectPool.Instance.GetObject<GameInputMoveToR>();
            moveToR.m_nPlayerIndex = m_nUserPlayerIndex;
            moveToR.m_Direction = FBS.MoveDirection.Down;

            RoomNetwork.Instance.Send(moveToR);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameInputMoveToR moveToR = ObjectPool.Instance.GetObject<GameInputMoveToR>();
            moveToR.m_nPlayerIndex = m_nUserPlayerIndex;
            moveToR.m_Direction = FBS.MoveDirection.Left;

            RoomNetwork.Instance.Send(moveToR);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameInputMoveToR moveToR = ObjectPool.Instance.GetObject<GameInputMoveToR>();
            moveToR.m_nPlayerIndex = m_nUserPlayerIndex;
            moveToR.m_Direction = FBS.MoveDirection.Right;

            RoomNetwork.Instance.Send(moveToR);
        }

        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(-1, -1, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(0, -1, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(1, -1, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(-1, 0, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(1, 0, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(-1, 1, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(0, 1, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
            rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
            rotationToR.m_Rotation = new Vector3(1, 1, 0);

            RoomNetwork.Instance.Send(rotationToR);
        }
    }

    private void Start()
    {
        m_nOldFrameRate = Application.targetFrameRate;
        Application.targetFrameRate = 30;

        RoomNetwork.Instance.ConnectToServer(Config.Instance.GetRoomServerIP(), Config.Instance.GetRoomServerPort(), OnConnected, OnRecvMessage);
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            while(m_fLastWorldInfoTime == -1)
            {
                yield return null;
            }

            UpdateTime();

            if (m_fLastUpdateTime < m_fElapsedTime)
            {
                UpdateWorld();

                m_fLastUpdateTime = m_fElapsedTime;

                m_bEntitySample = true;
            }

            yield return null;
        }
    }

    private void LateUpdate()
    {
        if (m_bEntitySample)
        {
            EntitySample();

            m_bEntitySample = false;
        }
    }

    private void EntitySample()
    {
        foreach (KeyValuePair<int, Entity> kv in m_dicEntity)
        {
            kv.Value.Sample();
        }
    }

    private void UpdateWorld()
    {
        ProcessWorldInfo();
    }

    private void UpdateTime()
    {
        float fTickInterval = 0.1f;     //  server tick interval (서버에서 받는 구조로 바꾸자)
        float fNetworkLatency = 0.04f;
        float fLimit = fTickInterval + fNetworkLatency;

        if (m_fLastWorldInfoTime > m_fElapsedTime)
        {
            if (m_fLastWorldInfoTime > m_fElapsedTime + Time.deltaTime)
            {
                float fDiff = m_fLastWorldInfoTime - m_fElapsedTime;
                if (fDiff > fLimit)
                {
                    float fFactor = (fDiff - fLimit) / fLimit + 1;

                    deltaTime = Time.deltaTime * fFactor;
                    if (m_fElapsedTime + deltaTime > m_fLastWorldInfoTime)
                    {
                        deltaTime = m_fLastWorldInfoTime - m_fElapsedTime;
                    }
                }
                else
                {
                    deltaTime = Time.deltaTime;
                }
            }
            else
            {
                deltaTime = m_fLastWorldInfoTime - m_fElapsedTime;
            }
        }
        else
        {
            deltaTime = 0;
        }

        m_fElapsedTime += deltaTime;
    }

    private void ProcessWorldInfo()
    {
        for (int sec = (int)m_fLastUpdateTime; sec <= (int)m_fElapsedTime; ++sec)
        {
            if (m_dicGameEvent.ContainsKey(sec))
            {
                foreach(IGameEvent iGameEvent in m_dicGameEvent[sec])
                {
                    if (iGameEvent.m_fEventTime < m_fLastUpdateTime)
                    {
                        continue;
                    }
                    else if (m_fElapsedTime < iGameEvent.m_fEventTime)
                    {
                        continue;
                    }
                    else if (m_dicProcessedGameEvent.ContainsKey(sec) && m_dicProcessedGameEvent[sec].Contains(iGameEvent))
                    {
                        continue;
                    }
                    else
                    {
                        ProcessGameEvent(iGameEvent);
                    }
                }
            }
        }
    }

    private void ProcessGameEvent(IGameEvent iGameEvent)
    {
        if (iGameEvent.GetEventType() == FBS.GameEventType.BehaviorStart)
        {
            GameEvent.BehaviorStart gameEvent = (GameEvent.BehaviorStart)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.BehaviorEnd)
        {
            GameEvent.BehaviorEnd gameEvent = (GameEvent.BehaviorEnd)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.StateStart)
        {
            GameEvent.StateStart gameEvent = (GameEvent.StateStart)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.StateEnd)
        {
            GameEvent.StateEnd gameEvent = (GameEvent.StateEnd)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.Position)
        {
            GameEvent.Position gameEvent = (GameEvent.Position)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.Rotation)
        {
            GameEvent.Rotation gameEvent = (GameEvent.Rotation)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.EntityCreate)
        {
            GameEvent.EntityCreate gameEvent = (GameEvent.EntityCreate)iGameEvent;

            GameObject goEntity = ObjectPool.Instance.GetGameObject("CharacterModel/Entity");
            Entity entity = goEntity.GetComponent<Entity>();
            entity.Initialize(gameEvent.m_EntityType, gameEvent.m_nEntityID, gameEvent.m_nMasterDataID);
            entity.SetPosition(gameEvent.m_vec3Position);
            entity.SetRotation(gameEvent.m_vec3Rotation);

            m_dicEntity[gameEvent.m_nEntityID] = entity;
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.EntityDestroy)
        {
            GameEvent.EntityDestroy gameEvent = (GameEvent.EntityDestroy)iGameEvent;

            ObjectPool.Instance.ReturnGameObject(m_dicEntity[gameEvent.m_nEntityID].gameObject);

            m_dicEntity.Remove(gameEvent.m_nEntityID);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.CharacterAttack)
        {
            GameEvent.CharacterAttack gameEvent = (GameEvent.CharacterAttack)iGameEvent;

            m_dicEntity[gameEvent.m_nAttackedEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.CharacterRespawn)
        {
            GameEvent.CharacterRespawn gameEvent = (GameEvent.CharacterRespawn)iGameEvent;

            if (gameEvent.m_nEntityID == m_nUserEntityID)
            {
                //  Die effect off
                m_goGreyCover.SetActive(false);
                m_UICountTimer.Stop();
                m_UICountTimer.Hide();
            }

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.CharacterStatusChange)
        {
            GameEvent.CharacterStatusChange gameEvent = (GameEvent.CharacterStatusChange)iGameEvent;

            m_dicEntity[gameEvent.m_nEntityID].ProcessGameEvent(gameEvent);
        }

        if (!m_dicProcessedGameEvent.ContainsKey((int)iGameEvent.m_fEventTime))
        {
            m_dicProcessedGameEvent[(int)iGameEvent.m_fEventTime] = new HashSet<IGameEvent>();
        }

        m_dicProcessedGameEvent[(int)iGameEvent.m_fEventTime].Add(iGameEvent);
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

    private void StartGame()
    {
        m_fElapsedTime = 0f;
        deltaTime = 0;
        m_fLastUpdateTime = -0.001f;
        m_fLastWorldInfoTime = -1;
        m_dicGameEvent.Clear();
        m_dicProcessedGameEvent.Clear();

        m_InputManager.Work(100, 500/*temp.. always 200, 200*/, m_CameraMain, OnClicked);
        m_RotationController.onHold += OnRotationControllerHold;

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

    #region Network Message Handler
    private void OnRecvMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == EnterRoomToC.MESSAGE_ID)
        {
            OnEnterRoomToC((EnterRoomToC)iMsg);
        }
        else if (iMsg.GetID() == PlayerEnterRoomToC.MESSAGE_ID)
        {

        }
        else if (iMsg.GetID() == GameStartToC.MESSAGE_ID)
        {
            OnGameStartToC((GameStartToC)iMsg);
        }
        else if (iMsg.GetID() == WorldInfoToC.MESSAGE_ID)
        {
            OnWorldInfoToC((WorldInfoToC)iMsg);
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
                GameObject goCharacter = ObjectPool.Instance.GetGameObject("CharacterModel/Character");
                Character character = goCharacter.GetComponent<Character>();
                character.Initialize(FBS.Data.EntityType.Character, player.EntityID, player.MasterDataID);
                character.InitStatus(new CharacterStatus(player.Status));
                m_dicEntity[player.EntityID] = character;

                if (player.PlayerIndex == m_nUserPlayerIndex)
                {
                    m_nUserEntityID = player.EntityID;

                    m_CameraController.SetTarget(character.GetUITransform());
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
        StartGame();
    }

    private void OnWorldInfoToC(WorldInfoToC msg)
    {
        m_fLastWorldInfoTime = msg.m_fEndTime;

        foreach(IGameEvent iGameEvent in msg.m_listGameEvent)
        {
            int nSec = (int)iGameEvent.m_fEventTime;

            if (!m_dicGameEvent.ContainsKey(nSec))
                m_dicGameEvent.Add(nSec, new List<IGameEvent>());

            m_dicGameEvent[nSec].Add(iGameEvent);

            if (iGameEvent.m_fEventTime < m_fLastUpdateTime)
            {
                Debug.LogError(string.Format("EventTime of GameEvent < LastUpdateTime!! EventTime : {0}, LastUpdateTime : {1}", iGameEvent.m_fEventTime, m_fLastUpdateTime));
                Debug.LogError(iGameEvent.ToString());
            }
        }
    }

    private void OnTickInfoToC(TickInfoToC msg)
    {
        if(msg.m_listPlayerInput.Count > 0)
            Debug.Log(msg.ToString());
    }
    #endregion

    #region Event Handler
    private void OnClicked(Vector3 vec3Pos)
    {
        if (!GetUserCharacter().IsAlive())
            return;

        //        GameEventRunToR runToR = new GameEventRunToR();
        //        runToR.m_nPlayerIndex = m_nUserPlayerIndex;
        //        runToR.m_vec3Dest = vec3Pos;

        GameInputMoveToR moveToR = new GameInputMoveToR();
        moveToR.m_nPlayerIndex = m_nUserPlayerIndex;
        moveToR.m_Direction = FBS.MoveDirection.Up;

        RoomNetwork.Instance.Send(moveToR);
    }

    private void OnRotationControllerHold(Vector2 vec2Direction)
    {
        float y = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(vec2Direction.x, 0, vec2Direction.y));

        if (vec2Direction.x < 0)
        {
            y = 360 - y;
        }

//        GameInputRotationToR rotationToR = ObjectPool.Instance.GetObject<GameInputRotationToR>();
//        rotationToR.m_nPlayerIndex = m_nUserPlayerIndex;
//        rotationToR.m_Rotation = new Vector3(0, y, 0);
//
//        RoomNetwork.Instance.Send(rotationToR);

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
}
