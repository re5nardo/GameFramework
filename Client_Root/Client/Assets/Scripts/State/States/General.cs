using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class General : IState
    {
		private ParticleSystem m_ParticleSystem = null;

        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.State masterState = null;
            MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref masterState);

            m_fLength = masterState.m_fLength;
			m_strFxName = masterState.m_strFxName;

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
			if (m_nStartTick == nUpdateTick)
            {
				GameObject goParticle = ObjectPool.Instance.GetGameObject(string.Format("Effect/{0}", m_strFxName));

                goParticle.transform.SetParent(m_Entity.GetModelTransform());
                goParticle.transform.localPosition = Vector3.zero;
                goParticle.transform.localRotation = Quaternion.identity;
                goParticle.transform.localScale = Vector3.one * 3;

                m_ParticleSystem = goParticle.GetComponent<ParticleSystem>();

                m_ParticleSystem.Simulate(m_fTickInterval, true, true, true);
            }
            else
            {
				if(m_ParticleSystem != null)
				{
					m_ParticleSystem.Simulate(m_fTickInterval, true, false, true);
				}
            }

			if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                if (m_ParticleSystem != null)
                {
                    ObjectPool.Instance.ReturnGameObject(m_ParticleSystem.gameObject);

                    m_ParticleSystem = null;
                }

                m_Entity.RemoveState(m_nMasterDataID, nUpdateTick);
            }
        }

        public override void OnCollision(IEntity other, int nTick)
        {
			if(IGameRoom.Instance.IsPredictMode())
    			return;
        }
    }
}