using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class Boost : IState
    {
        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.State masterState = null;
            MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref masterState);

            m_fLength = masterState.m_fLength;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            Character character = m_Entity as Character;

            if (nUpdateTick == m_nStartTick)
            {
                character.PlusMoveSpeed(5);
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                character.MinusMoveSpeed(5);

                m_Entity.RemoveState(m_nMasterDataID, nUpdateTick);
            }
        }

        public override void OnCollision(IEntity other, int nTick)
        {

        }
    }
}