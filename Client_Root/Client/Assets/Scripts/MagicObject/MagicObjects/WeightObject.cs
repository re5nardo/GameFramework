﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicObject
{
    public class WeightObject : IMagicObject
    {
        public override void Initialize(int nCasterID, int nMagicID, int nID, int nMasterDataID, float fTickInterval)
        {
            m_nCasterID = nCasterID;
            m_nMagicID = nMagicID;
            m_nID = nID;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.MagicObject masterMagicObject = null;
            MasterDataManager.Instance.GetData<MasterData.MagicObject>(nMasterDataID, ref masterMagicObject);

            m_fLength = masterMagicObject.m_fLength;

            m_goModel = ObjectPool.Instance.GetGameObject("MagicObject/" + masterMagicObject.m_strModelResName);

            m_goModel.transform.localPosition = Vector3.zero;
            m_goModel.transform.localRotation = Quaternion.identity;
            m_goModel.transform.localScale = Vector3.one;

            m_trModel = m_goModel.transform;
            m_ModelRigidbody = m_goModel.GetComponent<Rigidbody>();
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                if(m_goModel != null)
                {
                    ObjectPool.Instance.ReturnGameObject(m_goModel);

                    m_goModel = null;
                    m_trModel = null;
                    m_ModelRigidbody = null;
                }

                BaeGameRoom2.Instance.DestroyMagicObject(this);
            }
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
			if(BaeGameRoom2.Instance.IsPredictMode())
    			return;

            if (collisionInfo.gameObject.layer == GameObjectLayer.CHARACTER)
            {
                Character character = collisionInfo.gameObject.GetComponentInParent<Character>();

                if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Invincible) || character.GetID() == m_nCasterID)
                    return;

                character.OnAttacked(m_nCasterID, 1, BaeGameRoom2.Instance.GetCurrentTick());
            }
        }

        private void OnTriggerEnter(Collider colliderInfo)
        {
			if(BaeGameRoom2.Instance.IsPredictMode())
    			return;

            if (colliderInfo.gameObject.layer == GameObjectLayer.CHARACTER)
            {
                Character character = colliderInfo.gameObject.GetComponentInParent<Character>();

                if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Invincible) || character.GetID() == m_nCasterID)
                    return;

                character.OnAttacked(m_nCasterID, 1, BaeGameRoom2.Instance.GetCurrentTick());
            }
        }
    }
}