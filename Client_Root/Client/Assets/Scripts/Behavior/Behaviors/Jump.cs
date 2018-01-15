using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Jump : IBehavior
    {
        public override void Initialize(IEntity entity, int nMasterDataID)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (nUpdateTick == m_nStartTick)
            {
                Vector3 offset = m_Entity.GetForward().normalized * 5/*m_pEntity.GetSpeed()*/;

                m_Entity.m_vec3Velocity.x = offset.x;
                m_Entity.m_vec3Velocity.y += 10;
                m_Entity.m_vec3Velocity.z = offset.z;
            }
            else if(m_Entity.IsGrounded())
            {
                m_Entity.m_vec3Velocity.x = 0;
                m_Entity.m_vec3Velocity.z = 0;

                Stop();
            }
        }
    }
}