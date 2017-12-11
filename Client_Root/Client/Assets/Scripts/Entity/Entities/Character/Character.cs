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
        else if (iGameEvent.GetEventType() == FBS.GameEventType.CharacterStatusChange)
        {
            GameEvent.CharacterStatusChange gameEvent = (GameEvent.CharacterStatusChange)iGameEvent;

            ProcessCharacterStatusChange(gameEvent);
        }
    }

    private void ProcessCharacterAttack(GameEvent.CharacterAttack gameEvent)
    {
        Debug.Log(gameEvent.ToString());

        m_CurrentStatus.m_nHP -= gameEvent.m_nDamage;

        if (BaeGameRoom.Instance.GetUserEntityID() == gameEvent.m_nAttackedEntityID)
        {
            if (m_CurrentStatus.m_nHP <= 0)
            {
                BaeGameRoom.Instance.OnPlayerDie(gameEvent.m_nAttackedEntityID, gameEvent.m_nAttackingEntityID);
            }

            BaeGameRoom.Instance.OnUserStatusChanged(m_CurrentStatus);
        }
    }

    private void ProcessCharacterRespawn(GameEvent.CharacterRespawn gameEvent)
    {
        m_CurrentStatus.m_nHP = m_OriginalStatus.m_nHP;

        m_EntityUI.SetPosition(gameEvent.m_vec3Position);

        if (BaeGameRoom.Instance.GetUserEntityID() == gameEvent.m_nEntityID)
        {
            BaeGameRoom.Instance.OnUserStatusChanged(m_CurrentStatus);
        }
    }

    private void ProcessCharacterStatusChange(GameEvent.CharacterStatusChange gameEvent)
    {
        if (gameEvent.m_strStatusField == "HP")
        {
            m_CurrentStatus.m_nHP += (int)gameEvent.m_fValue;
        }
        else if (gameEvent.m_strStatusField == "MP")
        {
            m_CurrentStatus.m_nMP += (int)gameEvent.m_fValue;
        }
        else if (gameEvent.m_strStatusField == "MovePoint")
        {
            m_CurrentStatus.m_fMovePoint += gameEvent.m_fValue;
        }

        if (BaeGameRoom.Instance.GetUserEntityID() == gameEvent.m_nEntityID)
        {
            BaeGameRoom.Instance.OnUserStatusChanged(m_CurrentStatus);
        }
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
