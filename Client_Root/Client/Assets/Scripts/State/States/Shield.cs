using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class Shield : IState
    {
        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.State masterState = null;
            MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref masterState);

            m_fLength = masterState.m_fLength;

            m_listCoreState.Add(CoreState.CoreState_Invincible);
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                m_Entity.RemoveState(m_nMasterDataID, nUpdateTick);
            }
        }

        public override void OnCollision(IEntity other, int nTick)
        {

        }
    }
}