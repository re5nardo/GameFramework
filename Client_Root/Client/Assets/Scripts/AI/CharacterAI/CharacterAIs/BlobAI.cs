using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAI : ICharacterAI
{
    private const int SPAWN_TICK = 0;
    private const int FIRE_COOLTIME = 5;
    private const int FIRE_BEHAVIOR_ID = 9;

    private int m_nCoolTime = FIRE_COOLTIME;

    public override void Initialize(int nMasterDataID)
    {
        base.Initialize(nMasterDataID);

        m_nMasterDataID = nMasterDataID;

//        btVector3& vec3StartPosition, btVector3& vec3StartRotation
    }

    protected override void UpdateBody(int nUpdateTick)
    {
        if ((nUpdateTick - m_nStartTick) == SPAWN_TICK)
        {
            int nEntityID = 0;

            MasterData.Character masterCharacter = null;
            MasterDataManager.Instance.GetData<MasterData.Character>(m_nMasterDataID, ref masterCharacter);

            CharacterStatus status = new CharacterStatus(masterCharacter.m_nHP, masterCharacter.m_nHP, masterCharacter.m_nMP, masterCharacter.m_nMP, masterCharacter.m_fMaximumSpeed, 0, masterCharacter.m_fMPChargeRate, 0);

            BaeGameRoom2.Instance.CreateCharacter(m_nMasterDataID, ref nEntityID, ref m_character, Character.Role.Disturber, status);
        }

        if (m_character != null)
        {
            if (!m_character.GetBehavior(FIRE_BEHAVIOR_ID).IsActivated() && m_nCoolTime == 0)
            {
                m_character.GetBehavior(FIRE_BEHAVIOR_ID).StartTick(BaeGameRoom2.Instance.GetTickInterval(), nUpdateTick);
                m_character.GetBehavior(FIRE_BEHAVIOR_ID).UpdateTick(nUpdateTick);

                m_nCoolTime = FIRE_COOLTIME;
            }
            else
            {
                m_nCoolTime--;
            }
        }
    }
}
