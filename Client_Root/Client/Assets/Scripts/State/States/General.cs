﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class General : IState
    {
        private int m_nLifespan;

        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.State masterState = null;
            MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref masterState);

            m_fLength = masterState.m_fLength;

            foreach(string coreState in masterState.m_listCoreState)
            {
                if (coreState == "Invincible")
                {
                    m_listCoreState.Add(CoreState.CoreState_Invincible);
                }
                else if (coreState == "ChallengerDisturbing")
                {
                    m_listCoreState.Add(CoreState.CoreState_ChallengerDisturbing);
                }
                else if (coreState == "Faint")
                {
                    m_listCoreState.Add(CoreState.CoreState_Faint);
                }
            }

            if (m_fLength == -1)
            {
                m_nLifespan = -1;
            }
            else
            {
                m_nLifespan = (int)(m_fLength / m_fTickInterval);
            }
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nLifespan != -1 && nUpdateTick == m_nStartTick + m_nLifespan)
            {
                m_Entity.RemoveState(m_nMasterDataID, nUpdateTick);
            }
        }

        public override void OnCollision(IEntity other, int nTick)
        {

        }
    }
}