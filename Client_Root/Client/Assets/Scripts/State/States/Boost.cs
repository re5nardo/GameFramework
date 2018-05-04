using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class Boost : IState
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
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nStartTick == nUpdateTick)
            {
                Character character = m_Entity as Character;

                character.PlusMoveSpeed(5);

                GameObject goParticle = ObjectPool.Instance.GetGameObject("Effect/Toxic_Aura_01");

                goParticle.transform.SetParent(m_Entity.GetModelTransform());
                goParticle.transform.localPosition = Vector3.zero;
                goParticle.transform.localRotation = Quaternion.identity;
                goParticle.transform.localScale = Vector3.one * 3;

                m_ParticleSystem = goParticle.GetComponent<ParticleSystem>();

                m_ParticleSystem.Simulate(m_fTickInterval, true, true, true);
            }
            else
            {
                m_ParticleSystem.Simulate(m_fTickInterval, true, false, true);
            }


            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                Character character = m_Entity as Character;

                character.MinusMoveSpeed(5);

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
			if(BaeGameRoom2.Instance.IsPredictMode())
    			return;
        }
    }
}