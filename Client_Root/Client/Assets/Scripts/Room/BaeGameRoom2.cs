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
    [SerializeField] private InputController        m_JumpController = null;
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

	private const int LATENCY_DEVIATION_LIMIT = 20;		//	ms

    private int m_nOldFrameRate = 0;
    private Vector3 m_vec3OldGravity;

    private List<MovingObject> m_listMovingObject = new List<MovingObject>();
    private List<ICharacterAI> m_listDisturber = new List<ICharacterAI>();

    private List<PlayerRankInfo> m_listPlayerRankInfo = new List<PlayerRankInfo>();

    private List<Character> m_listPlayerCharacter = new List<Character>();

	private int m_nPredictStartTick = 0;
	private System.DateTime m_PredictStartTime = System.DateTime.Now;

#region Room Logic
	private void Start()
	{
		Init();

		RoomNetwork.Instance.ConnectToServer(Config.Instance.GetRoomServerIP(), Config.Instance.GetRoomServerPort(), OnConnected, OnRecvMessage);
	}

	private void OnDestroy()
	{
		Clear();
	}

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

	protected override void Init()
	{
		m_nOldFrameRate = Application.targetFrameRate;
//        Application.targetFrameRate = 30;

        m_vec3OldGravity = Physics.gravity;
        Physics.gravity *= 4;

        m_RotationController.onHold += OnRotationControllerHold;

        foreach(GameItemButton gameItemButton in m_GameItemButtons)
        {
            gameItemButton.onClicked = OnGameItemButtonClicked;
        }

        m_JumpController.onReleased = OnJumpButtonClicked;
	}

	protected override void Clear()
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

        m_JumpController.onReleased = null;

        Application.targetFrameRate = m_nOldFrameRate;
        Physics.gravity = m_vec3OldGravity;
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

		EnterRoomToR msgToR = ObjectPool.Instance.GetObject<EnterRoomToR>();
        msgToR.m_strPlayerKey = SystemInfo.deviceUniqueIdentifier;
        msgToR.m_nAuthKey = 0;
        msgToR.m_nMatchID = 0;

        RoomNetwork.Instance.Send(msgToR);

//		ObjectPool.Instance.ReturnObject(msgToR);
    }
#endregion

#region Game Logic
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

    private int GetUserRank()
    {
        UpdatePlayerRank();

        int nCount = m_listPlayerCharacter.FindAll(x => x.GetCurrentHeight() > GetUserCharacter().GetCurrentHeight()).Count;

        return nCount + 1;
    }

	public override GameType GetGameType()
	{
		return GameType.GameType_Multi;
	}

	protected override void PreProcess()
    {
    }

	protected override bool ShouldWaitForProcess()
	{
		if(m_bPredictMode)
        {
			//	Stop predict mode
			if(m_nPredictStartTick <= m_nProcessibleTick)
			{
				Debug.LogWarning("[Normal Mode] m_nTick : " + m_nTick);

				m_bPredictMode = false;
				m_nTick = m_nPredictStartTick;

				//	Restore data
				foreach(MovingObject mObj in m_listMovingObject)
        		{
		            mObj.Restore();
        		}

				foreach(IEntity entity in m_dicEntity.Values)
        		{
					entity.Restore();
        		}

				m_GameItemManager.Restore();
        	}
    	}
    	else
        {
			if(m_nTick > m_nProcessibleTick)
			{
				//	Start predict mode
				if((System.DateTime.Now -  m_LastProcessTime).TotalMilliseconds > m_fTickInterval * 1000 + LATENCY_DEVIATION_LIMIT)
				{
					Debug.LogWarning("[Predict Mode] m_nTick : " + m_nTick);

					m_bPredictMode = true;
					m_nPredictStartTick = m_nTick;
					m_PredictStartTime = System.DateTime.Now;

					//	Save data
					foreach(MovingObject mObj in m_listMovingObject)
			        {
			            mObj.Save();
			        }

					foreach(IEntity entity in m_dicEntity.Values)
			        {
			            entity.Save();
			        }

					m_GameItemManager.Save();
				}
				else
				{
                    return true;
				}
			}
    	}

    	return false;
	}

	protected override void Draw()
    {
        base.Draw();

        m_lbInfo.text = string.Format("{0} / {1}\n현재 높이 : {2}m\n최고 높이 : {3}m", GetUserRank(), m_dicPlayerEntity.Count, (int)GetUserCharacter().GetCurrentHeight(), (int)GetUserCharacter().GetBestHeight());

        SetRemainTime((m_nEndTick - m_nTick) * m_fTickInterval);
    }

	protected override void UpdateWorld()
    {
        foreach(MovingObject mObj in m_listMovingObject)
        {
            mObj.UpdateTick(m_nTick); 
        }

        foreach(ICharacterAI disturber in m_listDisturber)
        {
            disturber.UpdateTick(m_nTick);
        }

       base.UpdateWorld();

        m_GameItemManager.UpdateTick(m_nTick);

        //  Copy values because m_listMagic can be modified during iterating
        IMagic[] magics = new IMagic[m_listMagic.Count];
        m_listMagic.CopyTo(magics, 0);
        foreach(IMagic magic in magics)
        {
            magic.UpdateTick(m_nTick);
        }

        //  Copy values because m_listMagicObject can be modified during iterating
        IMagicObject[] magicObjects = new IMagicObject[m_listMagicObject.Count];
        m_listMagicObject.CopyTo(magicObjects, 0);
        foreach(IMagicObject magicObject in magicObjects)
        {
            magicObject.UpdateTick(m_nTick);
        }
    }

	protected override void ResetGame()
    {
		base.ResetGame();

        m_listMovingObject.Clear();
        m_listDisturber.Clear();
        m_listPlayerRankInfo.Clear();
    }

	protected override IEnumerator PrepareGame()
    {
        //  prepare for game
        yield return SceneManager.LoadSceneAsync("TestMap3"/*temp.. always TestMap*/, LoadSceneMode.Additive);

        m_InputManager.Work(100, 500/*temp.. always 200, 200*/, m_CameraMain, OnClicked);

		PreparationStateToR preparationStateToR = ObjectPool.Instance.GetObject<PreparationStateToR>();
        preparationStateToR.m_fState = 1.0f;

        RoomNetwork.Instance.Send(preparationStateToR);

//		ObjectPool.Instance.ReturnObject(preparationStateToR);
    }

	protected override int GetCountToProcess()
	{
		int nCountToProcess = 0;
        if(m_bPredictMode)
        {
			int nCount = (int)((System.DateTime.Now - m_PredictStartTime).TotalSeconds / m_fTickInterval);

			nCountToProcess = nCount - (m_nTick - m_nPredictStartTick);
        }
        else
        {
			nCountToProcess = m_nProcessibleTick - m_nTick + 1;
        }

		return nCountToProcess;
	}

	protected override void OnGameEnd()
	{
		GameResultToR resultToR = ObjectPool.Instance.GetObject<GameResultToR>();
        resultToR.m_listPlayerRankInfo = GetPlayerRankInfos();

        RoomNetwork.Instance.Send(resultToR);

//		ObjectPool.Instance.ReturnObject(resultToR);
	}

    public int GetJustHigherRankPlayerEntityID(int nEntityID)
    {
        UpdatePlayerRank();

        float fHeight = GetCharacter(nEntityID).GetCurrentHeight();

        Character found = m_listPlayerCharacter.FindLast(x => x.GetCurrentHeight() > fHeight);

        if (found == null)
        {
            return -1;
        }

        return found.GetID();
    }

    private void UpdatePlayerRank()
    {
        m_listPlayerCharacter.Clear();

        foreach (int nID in m_dicPlayerEntity.Values)
        {
            Character character = m_dicEntity[nID] as Character;
            m_listPlayerCharacter.Add(character);
        }

        m_listPlayerCharacter.Sort(CompareHeight);
    }

    private int CompareHeight(Character a, Character b)
    {
        return a.GetCurrentHeight().CompareTo(b.GetCurrentHeight());
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

				MasterData.Character masterCharacter = null;
				MasterDataManager.Instance.GetData<MasterData.Character>(player.MasterDataID, ref masterCharacter);

				CharacterStatus status = new CharacterStatus(masterCharacter.m_nHP, masterCharacter.m_nHP, masterCharacter.m_nMP, masterCharacter.m_nMP, masterCharacter.m_fMaximumSpeed, masterCharacter.m_fMaximumSpeed, masterCharacter.m_fMPChargeRate, masterCharacter.m_nJumpCount, masterCharacter.m_nJumpCount, masterCharacter.m_fJumpRegenerationTime, 0);

				CreateCharacter(player.MasterDataID, ref nEntityID, ref character, Character.Role.Challenger, status, player.PlayerIndex == m_nUserPlayerIndex);

                m_dicPlayerEntity[player.PlayerIndex] = nEntityID;
                m_dicEntityPlayer[nEntityID] = player.PlayerIndex;

                if (player.PlayerIndex == m_nUserPlayerIndex)
                {
                    m_CameraController.SetTarget(character.GetModelTransform());
                    m_CameraController.StartFollowTarget();

                    //m_SkillController.SetSkills(new List<int>(){0, 1, 2});

//                    m_lbHP.text = string.Format("x{0}", player.Status.HP);
//                    m_lbMP.text = string.Format("x{0}", player.Status.MP);
//                    m_lbMovePoint.text = string.Format("x{0}", player.Status.MovePoint);
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

		for (int i = 0; i < 200; ++i)
        {
        	string strObject = "";
        	if(Random.Range(0, 2) == 0)
        	{
				strObject = "Meteor_Fast";
        	}
        	else
        	{
				strObject = "Meteor_Slow";
        	}

			MovingObject movingObject = ObjectPool.Instance.GetGameObject(strObject).GetComponent<MovingObject>();

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

		m_nProcessibleTick = msg.m_nTick;
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

//		ObjectPool.Instance.ReturnObject(position);
//		ObjectPool.Instance.ReturnObject(inputToR);
    }

    private void OnRotationControllerHold(Vector2 vec2Direction)
    {
        if (vec2Direction == Vector2.zero)
            return;

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

//		ObjectPool.Instance.ReturnObject(rotation);
//		ObjectPool.Instance.ReturnObject(inputToR);
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

//		ObjectPool.Instance.ReturnObject(inputGameItem);
//		ObjectPool.Instance.ReturnObject(inputToR);
    }
        
    private void OnJumpButtonClicked(float fTime)
    {
        if (!GetUserCharacter().IsJumpable())
            return;

        PlayerInputToR inputToR = ObjectPool.Instance.GetObject<PlayerInputToR>();
        inputToR.m_Type = FBS.PlayerInputType.Position;

        PlayerInput.Position position = ObjectPool.Instance.GetObject<PlayerInput.Position>();
        position.m_nEntityID = m_nUserEntityID;
        position.m_vec3Position = Vector3.zero;

        inputToR.m_Data = position.Serialize();

        RoomNetwork.Instance.Send(inputToR);

//		ObjectPool.Instance.ReturnObject(position);
//		ObjectPool.Instance.ReturnObject(inputToR);
    }
#endregion

#region Game Event Handler
    public override void OnPlayerDie(int nKilledEntityID, int nKillerEntityID)
    {
        if (nKilledEntityID == m_nUserEntityID)
        {
            MasterData.Behavior behaviorMasterData = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(MasterDataDefine.BehaviorID.DIE, ref behaviorMasterData);

            //  Die Effect On
            m_goGreyCover.SetActive(true);
            m_UICountTimer.Work(behaviorMasterData.m_fLength);
            m_UICountTimer.Show();
        }
    }

    public override void OnPlayerRespawn(int nEntityID)
    {
        if (nEntityID == m_nUserEntityID)
        {
            //  Die effect off
            m_goGreyCover.SetActive(false);
            m_UICountTimer.Stop();
            m_UICountTimer.Hide();
        }
    }

    public override void OnUserStatusChanged(CharacterStatus status)
    {
        m_lbHP.text = string.Format("x{0}", status.m_nHP);
        m_lbMP.text = string.Format("x{0}", status.m_nMP);
        m_lbMovePoint.text = string.Format("x{0}", (int)status.m_fMovePoint);
    }

    public override void OnUserGameItemChanged(GameItem[] gameItems)
    {
        for (int i = 0; i < m_GameItemButtons.Length; ++i)
        {
            m_GameItemButtons[i].SetData(gameItems[i]);
        }
    }
#endregion
}