using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    private CharacterStat m_DefaultStat;
    private CharacterStat m_CurrentStat;

    public override void Initialize(FBS.Data.EntityType entityType, int nID, int nMasterDataID)
    {
        MasterData.Character characterMasterData = null;
        MasterDataManager.Instance.GetData<MasterData.Character>(nMasterDataID, ref characterMasterData);

        InitStat(new CharacterStat(characterMasterData.m_nHP, characterMasterData.m_nMP, 1, characterMasterData.m_fMaximumSpeed, 1));

        base.Initialize(entityType, nID, nMasterDataID);
    }

    public override void ProcessGameEvent(IGameEvent iGameEvent)
    {
        base.ProcessGameEvent(iGameEvent);

        if (iGameEvent.GetEventType() == FBS.GameEventType.CharacterAttack)
        {
            GameEvent.CharacterAttack gameEvent = (GameEvent.CharacterAttack)iGameEvent;

            ProcessCharacterAttack(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.CharacterRespawn)
        {
            GameEvent.CharacterRespawn gameEvent = (GameEvent.CharacterRespawn)iGameEvent;

            ProcessCharacterRespawn(gameEvent);
        }
    }

    private void ProcessCharacterAttack(GameEvent.CharacterAttack gameEvent)
    {
        Debug.Log(gameEvent.ToString());

        m_CurrentStat.m_nHP -= gameEvent.m_nDamage;

        if (m_CurrentStat.m_nHP <= 0)
        {
            BaeGameRoom.Instance.OnPlayerDie(gameEvent.m_nAttackedEntityID, gameEvent.m_nAttackingEntityID);
        }
    }

    private void ProcessCharacterRespawn(GameEvent.CharacterRespawn gameEvent)
    {
        m_CurrentStat.m_nHP = m_DefaultStat.m_nHP;

        m_EntityUI.SetPosition(gameEvent.m_vec3Position);
    }

    public void InitStat(CharacterStat stat)
    {
        m_DefaultStat = stat;
        m_CurrentStat = stat;
    }

    public bool IsAlive()
    {
        return m_CurrentStat.m_nHP > 0;
    }
}
