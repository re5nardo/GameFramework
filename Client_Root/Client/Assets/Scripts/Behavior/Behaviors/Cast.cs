using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Cast : IBehavior
    {
        private int m_nEndTick = 0;

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

            m_nEndTick = (int)(m_fLength / m_fTickInterval) - 1;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            foreach (MasterData.Behavior.Action action in m_listAction)
            {
                int nTick = (int)(action.m_fTime / m_fTickInterval);

                if (nUpdateTick == nTick + m_nStartTick)
                {
                    if (action.m_strID == "Cast")
                    {
                        
                    }
                }
            }

            if (nUpdateTick == m_nEndTick + m_nStartTick)
            {
                Stop();
            }
        }
    }
}