using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    private CharacterStatus m_OriginalStatus;
    private CharacterStatus m_CurrentStatus;

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

        m_CurrentStatus.m_nHP -= gameEvent.m_nDamage;

        if (m_CurrentStatus.m_nHP <= 0)
        {
            BaeGameRoom.Instance.OnPlayerDie(gameEvent.m_nAttackedEntityID, gameEvent.m_nAttackingEntityID);
        }
    }

    private void ProcessCharacterRespawn(GameEvent.CharacterRespawn gameEvent)
    {
        m_CurrentStatus.m_nHP = m_OriginalStatus.m_nHP;

        m_EntityUI.SetPosition(gameEvent.m_vec3Position);
    }

    public void InitStatus(CharacterStatus status)
    {
        m_OriginalStatus = status;
        m_CurrentStatus = status;
    }

    public bool IsAlive()
    {
        return m_CurrentStatus.m_nHP > 0;
    }
}
