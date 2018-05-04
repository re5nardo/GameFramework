using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Move : IBehavior
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

			m_bPredictPlay = true;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if(nUpdateTick - m_nStartTick >= 5)
            {
                Stop();
            }
            else
            {
				if (m_Entity.HasCoreState(CoreState.CoreState_Faint))
                	return;

				Vector3 offset = m_Entity.GetForward().normalized * m_Entity.GetSpeed();

				m_Entity.Move(offset * m_fTickInterval);
            }
        }
    }
}