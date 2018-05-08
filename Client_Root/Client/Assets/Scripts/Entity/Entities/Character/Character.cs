using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : IEntity
{
    public enum Role
    {
        Challenger = 0,
        Disturber,
    };

    private Role m_Role = Role.Challenger;

    //    protected List<ISkill> m_listSkill;
    private Dictionary<int,int> m_dicGameItemEffect = new Dictionary<int,int>();
    private GameItem[] m_GameItems = new GameItem[2];

    protected CharacterStatus m_OriginalStatus;
    protected CharacterStatus m_CurrentStatus;

    private int m_nJumpRegenerationTickInterval = 0;
    private int m_nJumpRegenerationTick = 0;

    public float m_fSpeedPercent = 100;

    private float m_fBestHeight = 0;

    public void SetRole(Role role)
    {
        m_Role = role;
    }

    public Role GetRole()
    {
        return m_Role;
    }

    public override void Initialize(int nID, int nMasterDataID, params object[] param)
    {
        m_nID = nID;
        m_nMasterDataID = nMasterDataID;
        m_Role = (Role)param[0];
		InitStatus((CharacterStatus)param[1]);

		m_nJumpRegenerationTickInterval = (int)(m_CurrentStatus.m_fJumpRegenerationTime / BaeGameRoom2.Instance.GetTickInterval());

        MasterData.Character masterCharacter = null;
        MasterDataManager.Instance.GetData<MasterData.Character>(m_nMasterDataID, ref masterCharacter);

        //        foreach(int nSkillID in pMasterCharacter.m_listSkillID)
        //        {
        //            Factory.Instance.CreateBehavior(this, nSkillID);
        //        }

        foreach(KeyValuePair<int, int> kv in masterCharacter.m_dicGameItemEffect)
        {
            m_dicGameItemEffect.Add(kv.Key, kv.Value);
        }

        foreach(int nBehaviorID in masterCharacter.m_listBehaviorID)
        {
            IBehavior behavior = Factory.Instance.CreateBehavior(nBehaviorID);

            if (behavior == null)
            {
                Debug.LogWarning("behavior is null! nBehaviorID : " + nBehaviorID);
                continue;
            }

            behavior.Initialize(this, nBehaviorID, BaeGameRoom2.Instance.GetTickInterval());

            m_listBehavior.Add(behavior);
        }

        GameObject goEntityUI = ObjectPool.Instance.GetGameObject("CharacterModel/EntityUI");
        goEntityUI.transform.parent = gameObject.transform;

        EntityUI entityUI = goEntityUI.GetComponent<EntityUI>();
        entityUI.Initialize(FBS.Data.EntityType.Character, nID, nMasterDataID);

        m_EntityUI = entityUI;
    }

    public override float GetSpeed()
    {
        return m_CurrentStatus.m_fSpeed * (m_fSpeedPercent / 100);
    }

    public override float GetMaximumSpeed()
    {
        return m_CurrentStatus.m_fMaximumSpeed;
    }

    public override FBS.Data.EntityType GetEntityType()
    {
        return FBS.Data.EntityType.Character;
    }

    public override void NotifyGameEvent(IGameEvent gameEvent)
    {
    }

    public void UpdateSkills(long lUpdateTime)
    {
    }

    public override void LateUpdateWorld(int nUpdateTick)
    {
        m_fBestHeight = Mathf.Max(m_fBestHeight, GetCurrentHeight());   //  Timing correct?

		if (m_CurrentStatus.m_nJumpCount < m_CurrentStatus.m_nMaximumJumpCount)
        {
			m_nJumpRegenerationTick++;

			if (m_nJumpRegenerationTick == m_nJumpRegenerationTickInterval)
            {
				m_CurrentStatus.m_nJumpCount++;
				m_nJumpRegenerationTick = 0;
            }
        }
        else
        {
			m_nJumpRegenerationTick = 0;
        }

        if (HasCoreState(CoreState.CoreState_Faint))
            return;

        if (m_nDefaultBehaviorID != -1 && !IsBehavioring() && GetBehavior(m_nDefaultBehaviorID) != null && IsAlive())
        {
            GetBehavior(m_nDefaultBehaviorID).StartTick(nUpdateTick);
        }
    }

    //    public ISkill* GetSkill(int nID);
    //    public List<ISkill*> GetAllSkills();
    //    public List<ISkill*> GetActivatedSkills();
    //    public bool IsSkilling();

	private void InitStatus(CharacterStatus status)
    {
        m_OriginalStatus = status;
        m_CurrentStatus = status;
    }

    public int GetCurrentMP()
    {
        return m_CurrentStatus.m_nMP;
    }

    public void SetCurrentMP(int nMP)
    {
        m_CurrentStatus.m_nMP = nMP;
    }

    public void SetMoveSpeed(float fSpeed)
    {
        m_CurrentStatus.m_fSpeed = fSpeed;
    }

    public void PlusMoveSpeed(float fValue)
    {
        m_CurrentStatus.m_fSpeed += fValue;
    }

    public void MinusMoveSpeed(float fValue)
    {
        m_CurrentStatus.m_fSpeed -= fValue;
    }

	public int GetJumpCount()
    {
        return m_CurrentStatus.m_nJumpCount;
    }

	public int GetMaximumJumpCount()
    {
        return m_CurrentStatus.m_nMaximumJumpCount;
    }

    public void OnJump(int nTick)
    {
		if (!IsJumpable())
            return;

		m_CurrentStatus.m_nJumpCount--;

        GetBehavior(MasterDataDefine.BehaviorID.JUMP).StartTick(nTick);
    }

    public void OnAttacked(int nAttackingEntityID, int nDamage, int nTick)
    {
        if (HasCoreState(CoreState.CoreState_Invincible) || !IsAlive())
            return;

        m_CurrentStatus.m_nHP -= nDamage;

        if (m_CurrentStatus.m_nHP <= 0)
        {
            List<IBehavior> listActivatedBehavior = GetActivatedBehaviors();
            foreach(IBehavior behavior in listActivatedBehavior)
            {
                behavior.Stop();
            }

            //  Die behavior
            IBehavior dieBehavior = GetBehavior(MasterDataDefine.BehaviorID.DIE);
            dieBehavior.StartTick(nTick);
            dieBehavior.UpdateTick(nTick);

            //  Faint state
            IState state = Factory.Instance.CreateState(MasterDataDefine.StateID.FAINT);
            state.Initialize(this, MasterDataDefine.StateID.FAINT, BaeGameRoom2.Instance.GetTickInterval());

            AddState(state, nTick);

            state.StartTick(nTick);
            state.UpdateTick(nTick);

            BaeGameRoom2.Instance.OnPlayerDie(m_nID, nAttackingEntityID);
        }
    }

    public void OnRespawn(int nTick)
    {
        m_CurrentStatus.m_nHP = m_OriginalStatus.m_nHP;

        IState state = Factory.Instance.CreateState(MasterDataDefine.StateID.RESPAWN_INVINCIBLE);
        state.Initialize(this, MasterDataDefine.StateID.RESPAWN_INVINCIBLE, BaeGameRoom2.Instance.GetTickInterval());

        AddState(state, nTick);

        state.StartTick(nTick);
        state.UpdateTick(nTick);

        BaeGameRoom2.Instance.OnPlayerRespawn(m_nID);
    }

    public void OnMoved(float fDistance, long lTime)
    {
        int nBonusPoint = 100;
        float fMovePoint = fDistance * m_CurrentStatus.m_fMPChargeRate * 100;
        m_CurrentStatus.m_fMovePoint += fMovePoint;
        BaeGameRoom2.Instance.AddCharacterStatusChangeGameEvent(lTime / 1000.0f, m_nID, "MovePoint", "Move", fMovePoint);

        if (m_CurrentStatus.m_fMovePoint >= nBonusPoint)
        {
            int nGetCount = (int)(m_CurrentStatus.m_fMovePoint / (float)nBonusPoint);

            m_CurrentStatus.m_fMovePoint -= (nGetCount * nBonusPoint);
            BaeGameRoom2.Instance.AddCharacterStatusChangeGameEvent(lTime / 1000.0f, m_nID, "MovePoint", "Transfer", -nGetCount * nBonusPoint);

            m_CurrentStatus.m_nMP += nGetCount;
            BaeGameRoom2.Instance.AddCharacterStatusChangeGameEvent(lTime / 1000.0f, m_nID, "MP", "MovePoint", nGetCount);
        }
    }

    public bool IsAlive()
    {
        return m_CurrentStatus.m_nHP > 0;
    }

    public bool IsJumpable()
    {
		return IsAlive() && !HasCoreState(CoreState.CoreState_Faint) &&  m_CurrentStatus.m_nJumpCount > 0;
    }

    public float GetCurrentHeight()
    {
        return m_EntityUI.GetPosition().y;
    }

    public float GetBestHeight()
    {
        return m_fBestHeight;
    }

    public void OnGetGameItem(GameItem item)
    {
        for (int i = 0; i < m_GameItems.Length; ++i)
        {
            if (m_GameItems[i] == null)
            {
                m_GameItems[i] = item;

                if (BaeGameRoom2.Instance.GetUserEntityID() == m_nID)
                {
                    BaeGameRoom2.Instance.OnUserGameItemChanged(m_GameItems);
                }

                return;
            }
        }
    }

    public void OnUseGameItem(int nID)
    {
        GameItem target = null;
        for (int i = 0; i < m_GameItems.Length; ++i)
        {
            if (m_GameItems[i] != null && m_GameItems[i].GetID() == nID)
            {
                target = m_GameItems[i];
                m_GameItems[i] = null;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("[OnUseGameItem] target is null! nID : " + nID);
            return;
        }

        if (BaeGameRoom2.Instance.GetUserEntityID() == m_nID)
        {
            BaeGameRoom2.Instance.OnUserGameItemChanged(m_GameItems);
        }

        int nGameItemEffectID = m_dicGameItemEffect[target.GetMasterDataID()];

        MasterData.GameItemEffect masterGameItemEffect = null;
        MasterDataManager.Instance.GetData<MasterData.GameItemEffect>(nGameItemEffectID, ref masterGameItemEffect);

        if (masterGameItemEffect.m_Type == MasterData.GameItemEffect.Type.Behavior)
        {
            IBehavior targetBehavior = GetBehavior(masterGameItemEffect.m_nTargetID);
            targetBehavior.StartTick(BaeGameRoom2.Instance.GetCurrentTick());
            targetBehavior.UpdateTick(BaeGameRoom2.Instance.GetCurrentTick());
        }
    }
}