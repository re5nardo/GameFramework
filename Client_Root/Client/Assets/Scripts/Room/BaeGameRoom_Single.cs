using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaeGameRoom_Single : IGameRoom
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

	public static new BaeGameRoom_Single Instance
    {
        get
        {
			return (BaeGameRoom_Single)instance;
        }
    }

    private int m_nOldFrameRate = 0;
    private Vector3 m_vec3OldGravity;

    private List<MovingObject> m_listMovingObject = new List<MovingObject>();
    private List<ICharacterAI> m_listDisturber = new List<ICharacterAI>();

#region Room Logic
	private void Start()
	{
		Init();
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

        int nUserPlayerIndex = 0;
        int nMasterDataID = Random.Range(0, 3);

        m_nUserPlayerIndex = nUserPlayerIndex;

        int nEntityID = 0;
        Character character = null;

        MasterData.Character masterCharacter = null;
        MasterDataManager.Instance.GetData<MasterData.Character>(nMasterDataID, ref masterCharacter);

        CharacterStatus status = new CharacterStatus(masterCharacter.m_nHP, masterCharacter.m_nHP, masterCharacter.m_nMP, masterCharacter.m_nMP, masterCharacter.m_fMaximumSpeed, masterCharacter.m_fMaximumSpeed, masterCharacter.m_fMPChargeRate, masterCharacter.m_nJumpCount, masterCharacter.m_nJumpCount, masterCharacter.m_fJumpRegenerationTime, 0);

        CreateCharacter(nMasterDataID, ref nEntityID, ref character, Character.Role.Challenger, status, true);

        m_dicPlayerEntity[nUserPlayerIndex] = nEntityID;
        m_dicEntityPlayer[nEntityID] = nUserPlayerIndex;

        m_CameraController.SetTarget(character.GetModelTransform());
        m_CameraController.StartFollowTarget();

        StartCoroutine(PrepareGame());
    }

	protected override void Clear()
    {
        m_RotationController.onHold -= OnRotationControllerHold;

        foreach(GameItemButton gameItemButton in m_GameItemButtons)
        {
            gameItemButton.onClicked = null;
        }

        m_JumpController.onReleased = null;

        Application.targetFrameRate = m_nOldFrameRate;
        Physics.gravity = m_vec3OldGravity;
    }
#endregion

#region Game Logic
	public override GameType GetGameType()
	{
		return GameType.GameType_Single;
	}

	protected override void PreProcess()
    {
		m_nProcessibleTick = (int)((System.DateTime.Now - m_StartTime).TotalSeconds / m_fTickInterval);
    }

	protected override bool ShouldWaitForProcess()
	{
		if(m_nTick > m_nProcessibleTick)
        {
            return true;
        }

        return false;
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

	protected override void Draw()
    {
        base.Draw();

        m_lbInfo.text = string.Format("현재 높이 : {0}m\n최고 높이 : {1}m", (int)GetUserCharacter().GetCurrentHeight(), (int)GetUserCharacter().GetBestHeight());

        SetRemainTime((m_nEndTick - m_nTick) * m_fTickInterval);
    }

	protected override bool IsGameEnd()
	{
		return (m_nTick == m_nEndTick && !m_bPredictMode) || !GetUserCharacter().IsAlive();
	}

	protected override void ResetGame()
    {
    	base.ResetGame();

        m_listMovingObject.Clear();
        m_listDisturber.Clear();
    }

	protected override IEnumerator PrepareGame()
    {
        //  prepare for game
        yield return SceneManager.LoadSceneAsync("TestMap3"/*temp.. always TestMap*/, LoadSceneMode.Additive);

        m_InputManager.Work(100, 500/*temp.. always 200, 200*/, m_CameraMain, OnClicked);


        float fTickInterval = 0.025f;
        int nRandomSeed = 0;
        int nTimeLimit = 120;

        m_fTickInterval = fTickInterval;
        Random.InitState(nRandomSeed);
        m_nEndTick = (int)(nTimeLimit / m_fTickInterval) - 1;

        SetRemainTime(nTimeLimit);

		for (int i = 0; i < 200; ++i)
        {
        	string strObject = "";
        	if(Random.Range(0, 8) == 0)
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

	protected override int GetCountToProcess()
	{
		return m_nProcessibleTick - m_nTick + 1;
	}

	protected override void OnGameEnd()
	{
		Character chracter = GetUserCharacter();

		SingleGameResultPopup popup = PopupManager.Instance.ShowPopup("SingleGameResultPopup") as SingleGameResultPopup;
		popup.SetData(chracter.GetCurrentHeight(), chracter.GetBestHeight());
	}

    private void SetRemainTime(float fRemainTime)
    {
        int sec = (int)fRemainTime;
        int milliSec = (int)((fRemainTime - sec) * 1000);

        m_lbTime.text = string.Format("{0}:{1:000}", sec, milliSec);
    }
#endregion

#region Network Message Handler
    private void OnGameEndToC(GameEndToC msg)
    {
        Debug.LogWarning("GameEndToC!");
    }
#endregion

#region Event Handler
    private void OnClicked(Vector3 vec3Pos)
    {
        if (!GetUserCharacter().IsAlive())
            return;
 
        PlayerInput.Position position = new PlayerInput.Position();
        position.m_nEntityID = m_nUserEntityID;
        position.m_vec3Position = vec3Pos;

        int nTick = m_nTick + 1;
        if (!m_dicPlayerInput.ContainsKey(nTick))
        {
            m_dicPlayerInput[nTick] = new List<IPlayerInput>();
        }

        m_dicPlayerInput[nTick].Add(position);
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

        PlayerInput.Rotation rotation = new PlayerInput.Rotation();
        rotation.m_nEntityID = m_nUserEntityID;
        rotation.m_vec3Rotation = new Vector3(0, y, 0);

        int nTick = m_nTick + 1;
        if (!m_dicPlayerInput.ContainsKey(nTick))
        {
            m_dicPlayerInput[nTick] = new List<IPlayerInput>();
        }

        m_dicPlayerInput[nTick].Add(rotation);
    }

    private void OnGameItemButtonClicked(GameItem gameItem)
    {
        if (!GetUserCharacter().IsAlive())
            return;
        
        PlayerInput.GameItem inputGameItem = new PlayerInput.GameItem();
        inputGameItem.m_nPlayerIndex = m_nUserPlayerIndex;
        inputGameItem.m_nGameItemID = gameItem.GetID();

        int nTick = m_nTick + 1;
        if (!m_dicPlayerInput.ContainsKey(nTick))
        {
            m_dicPlayerInput[nTick] = new List<IPlayerInput>();
        }

        m_dicPlayerInput[nTick].Add(inputGameItem);
    }
        
    private void OnJumpButtonClicked(float fTime)
    {
        if (!GetUserCharacter().IsJumpable())
            return;

        PlayerInput.Position position = new PlayerInput.Position();
        position.m_nEntityID = m_nUserEntityID;
        position.m_vec3Position = Vector3.zero;

        int nTick = m_nTick + 1;
        if (!m_dicPlayerInput.ContainsKey(nTick))
        {
            m_dicPlayerInput[nTick] = new List<IPlayerInput>();
        }

        m_dicPlayerInput[nTick].Add(position);
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