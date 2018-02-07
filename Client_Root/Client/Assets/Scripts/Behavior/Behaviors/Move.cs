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
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (nUpdateTick == m_nStartTick)
            {
                Vector3 offset = m_Entity.GetForward().normalized * 5/*m_pEntity.GetSpeed()*/;

                m_Entity.m_vec3Velocity.x = offset.x;
                m_Entity.m_vec3Velocity.z = offset.z;
            }
            else if(nUpdateTick - m_nStartTick >= 5)
            {
                m_Entity.m_vec3Velocity.x = 0;
                m_Entity.m_vec3Velocity.z = 0;

                Stop();
            }

//            if (nUpdateTick - m_nStartTick >= 5)
//            {
//                Stop();
//            }
//            else
//            {
//                Vector3 offset = m_Entity.GetForward().normalized * 5/*m_pEntity.GetSpeed()*/ * m_fTickInterval;
//
//                m_Entity.Move(offset);
//            }
        }
    }
}