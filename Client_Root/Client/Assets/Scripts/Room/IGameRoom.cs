using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IGameRoom : MonoSingleton<IGameRoom>
{
	protected int m_nUserPlayerIndex = -1;
	protected int m_nUserEntityID = -1;
    private int m_nEntitySequence = 0;

	protected float m_fTickInterval = 0.025f;
	protected int m_nTick = 0;
	protected int m_nEndTick = 0;
	protected int m_nProcessibleTick = -1;
	protected Dictionary<int, List<IPlayerInput>> m_dicPlayerInput = new Dictionary<int, List<IPlayerInput>>();
	protected Dictionary<int, IEntity> m_dicEntity = new Dictionary<int, IEntity>();

	protected Dictionary<int, int> m_dicPlayerEntity = new Dictionary<int, int>();      //  key : PlayerIndex, value : EntityID
	protected Dictionary<int, int> m_dicEntityPlayer = new Dictionary<int, int>();      //  key : EntityID, value : PlayerIndex

	protected List<IMagic> m_listMagic = new List<IMagic>();
	protected List<IMagicObject> m_listMagicObject = new List<IMagicObject>();

    protected System.DateTime m_StartTime = System.DateTime.Now;
	protected System.DateTime m_LastProcessTime = System.DateTime.Now;

	protected bool m_bPredictMode = false;

#region Game Logic
	public abstract GameType GetGameType();
    protected abstract void Init();
	protected abstract void Clear();
	protected abstract IEnumerator PrepareGame();

	protected abstract void PreProcess();
	protected abstract bool ShouldWaitForProcess();
	protected abstract int GetCountToProcess();
	protected abstract void OnGameEnd();

	protected void StartGame()
    {
        m_StartTime = System.DateTime.Now;

        StartCoroutine(Loop());
    }

    //  should save client.version to meta file ( for checking compatibility)
	private IEnumerator Loop()
	{
		while (true)
        {
			PreProcess();

			if(ShouldWaitForProcess())
			{
				yield return null;
				continue;
			}

			int nCountToProcess = GetCountToProcess();
 
            for (int i = 0; i < nCountToProcess; ++i)
            {
                ProcessInput();

                UpdateWorld();

                LateUpdateWorld();

                Physics.Simulate(m_fTickInterval);

                Draw();		//  optional?

				if (IsGameEnd())
                {
					OnGameEnd();

                    yield break;
                }

                m_nTick++;
            }

			m_LastProcessTime = System.DateTime.Now;

            yield return new WaitForSeconds(m_fTickInterval);
        }
	}

	protected virtual void ProcessInput()
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

                    character.GetBehavior(MasterDataDefine.BehaviorID.ROTATION).StartTick(m_nTick, rotation.m_vec3Rotation);
                    character.GetBehavior(MasterDataDefine.BehaviorID.MOVE).StartTick(m_nTick);
                }
                else if (input.GetPlayerInputType() == FBS.PlayerInputType.Position)
                {
                    PlayerInput.Position position = input as PlayerInput.Position;

                    Character character = m_dicEntity[position.m_nEntityID] as Character;

                    if (!character.IsJumpable())
                        continue;

					character.OnJump(m_nTick);
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

	protected virtual void UpdateWorld()
    {
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
    }

	protected virtual void LateUpdateWorld()
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

	protected virtual void Draw()
    {
        foreach (IEntity entity in m_dicEntity.Values)
        {
            entity.Draw(m_nTick);
        }
    }

	protected virtual bool IsGameEnd()
	{
		return m_nTick == m_nEndTick && !m_bPredictMode;
	}

    protected virtual void ResetGame()
    {
        m_nUserPlayerIndex = -1;
        m_nUserEntityID = -1;

        m_fTickInterval = 0.025f;
        m_nTick = 0;
		m_nProcessibleTick = -1;
        m_dicPlayerInput.Clear();
        m_dicEntity.Clear();
        m_dicPlayerEntity.Clear();
        m_dicEntityPlayer.Clear();
		m_listMagic.Clear();
        m_listMagicObject.Clear();
    }

	public int GetUserPlayerIndex()
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

    public Character GetCharacter(int nID)
    {
        if (!m_dicEntity.ContainsKey(nID))
            return null;

        return (Character)m_dicEntity[nID];
    }

	public IEntity GetEntity(int nID)
    {
        if (!m_dicEntity.ContainsKey(nID))
            return null;

        return m_dicEntity[nID];
    }

	public List<Character> GetAllCharacters()
    {
		List<Character> listCharacter = new List<Character>();

		foreach (int nID in m_dicPlayerEntity.Values)
        {
            Character character = m_dicEntity[nID] as Character;
			listCharacter.Add(character);
        }

		return listCharacter;
    }

    public int GetCurrentTick()
    {
        return m_nTick;
    }

    public float GetTickInterval()
    {
        return m_fTickInterval;
    }

	public bool IsPredictMode()
	{
		return m_bPredictMode;
	}

    public void CreateCharacter(int nMasterDataID, ref int nEntityID, ref Character character, Character.Role role, CharacterStatus status, bool bUser)
    {
        nEntityID = m_nEntitySequence++;
        if (bUser)
        {
            m_nUserEntityID = nEntityID;
        }
            
        character = Factory.Instance.CreateCharacter(nMasterDataID);

		m_dicEntity[nEntityID] = character;

		character.Initialize(nEntityID, nMasterDataID, role, status);
    }

    public void CreateProjectile(int nMasterDataID, ref int nEntityID, ref Projectile projectile, int nCreatorID)
    {
        nEntityID = m_nEntitySequence++;

        projectile = Factory.Instance.CreateProjectile(nMasterDataID);
        projectile.Initialize(nEntityID, nMasterDataID);

        m_dicEntity[nEntityID] = projectile;
    }

    public void CreateMagic(int nMasterDataID, ref int nEntityID, ref IMagic magic, int nCasterID)
    {
        nEntityID = m_nEntitySequence++;

        magic = Factory.Instance.CreateMagic(nMasterDataID);
        magic.Initialize(nCasterID, nEntityID, nMasterDataID, m_fTickInterval);

        m_listMagic.Add(magic);
    }

    public void DestroyMagic(IMagic magic)
    {
        m_listMagic.Remove(magic);
    }

    public void CreateMagicObject(int nMasterDataID, ref int nEntityID, ref IMagicObject magicObject, int nCasterID, int nMagicID)
    {
        nEntityID = m_nEntitySequence++;

        magicObject = Factory.Instance.CreateMagicObject(nMasterDataID);
        magicObject.Initialize(nCasterID, nMagicID, nEntityID, nMasterDataID, m_fTickInterval);

        m_listMagicObject.Add(magicObject);
    }

    public void DestroyMagicObject(IMagicObject magicObject)
    {
        m_listMagicObject.Remove(magicObject);
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
#endregion

#region Game Event Handler
    public abstract void OnPlayerDie(int nKilledEntityID, int nKillerEntityID);
    public abstract void OnPlayerRespawn(int nEntityID);
    public abstract void OnUserStatusChanged(CharacterStatus status);
    public abstract void OnUserGameItemChanged(GameItem[] gameItems);
#endregion
}