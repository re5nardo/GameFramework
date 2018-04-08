using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class ChallengerDisturbing : IState
    {
        private Dictionary<int, float> m_dicDisturbingInfo = new Dictionary<int, float>();

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
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            float fTime = (nUpdateTick - m_nStartTick + 1) * m_fTickInterval;

            List<int> listToRemove = new List<int>();
            foreach(KeyValuePair<int, float> kv in m_dicDisturbingInfo)
            {
                if (fTime - kv.Value > 3)
                {
                    listToRemove.Add(kv.Key);
                }
            }

            foreach (int key in listToRemove)
            {
                m_dicDisturbingInfo.Remove(key);
            }

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