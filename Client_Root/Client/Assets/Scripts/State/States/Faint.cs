﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class Faint : IState
    {
        private int m_nLifeTime;

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
                m_nLifeTime = -1;
            }
            else
            {
                m_nLifeTime = (int)(m_fLength / m_fTickInterval);
            }
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nLifeTime != -1 && nUpdateTick == m_nStartTick + m_nLifeTime)
            {
                m_Entity.RemoveState(m_nMasterDataID, nUpdateTick);

                IState state = Factory.Instance.CreateState(StateID.RESPAWN_INVINCIBLE);
                state.Initialize(m_Entity, StateID.RESPAWN_INVINCIBLE, BaeGameRoom2.Instance.GetTickInterval());

                m_Entity.AddState(state, BaeGameRoom2.Instance.GetCurrentTick());

                state.StartTick(nUpdateTick);
                state.UpdateTick(nUpdateTick);
            }
        }

        public override void OnCollision(IEntity other, int nTick)
        {

        }
    }
}