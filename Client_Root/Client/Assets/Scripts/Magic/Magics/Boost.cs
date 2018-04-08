using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Boost : IMagic
    {
        private ParticleSystem m_ParticleSystem = null;

        public override void Initialize(int nCasterID, int nID, int nMasterDataID, float fTickInterval)
        {
            m_nCasterID = nCasterID;
            m_nID = nID;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.Magic masterMagic = null;
            MasterDataManager.Instance.GetData<MasterData.Magic>(nMasterDataID, ref masterMagic);

            m_fLength = masterMagic.m_fLength;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nStartTick == nUpdateTick)
            {
                GameObject goParticle = ObjectPool.Instance.GetGameObject("Effect/Magic_Ring_05");

                Character caster = BaeGameRoom2.Instance.GetCharacter(m_nCasterID);

                goParticle.transform.SetParent(caster.GetModelTransform());
                goParticle.transform.localPosition = Vector3.zero;
                goParticle.transform.localRotation = Quaternion.identity;
                goParticle.transform.localScale = Vector3.one;

                m_ParticleSystem = goParticle.GetComponent<ParticleSystem>();

                m_ParticleSystem.Simulate(m_fTickInterval, true, true, true);

                IState state = Factory.Instance.CreateState(MasterDataDefine.StateID.BOOST);
                state.Initialize(caster, MasterDataDefine.StateID.BOOST, BaeGameRoom2.Instance.GetTickInterval());

                caster.AddState(state, nUpdateTick);

                state.StartTick(nUpdateTick);
                state.UpdateTick(nUpdateTick);
            }
            else
            {
                m_ParticleSystem.Simulate(m_fTickInterval, true, false, true);
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                if (m_ParticleSystem != null)
                {
                    ObjectPool.Instance.ReturnGameObject(m_ParticleSystem.gameObject);

                    m_ParticleSystem = null;
                }

                BaeGameRoom2.Instance.DestroyMagic(this);
            }
        }
    }
}