using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Die : IBehavior
    {
        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.Behavior masterBehavior = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref masterBehavior);

            m_fLength = masterBehavior.m_fLength;
            m_strStringParams = masterBehavior.m_strStringParams;
            m_listAction = masterBehavior.m_listAction;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                Stop();

//                BaeGameRoom2.Instance.CharacterDieEnd(m_Entity.GetID());

                Character character = m_Entity as Character;

                character.OnRespawn(nUpdateTick);
            }
        }
    }
}