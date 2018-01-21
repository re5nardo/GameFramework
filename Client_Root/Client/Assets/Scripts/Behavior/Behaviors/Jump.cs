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
                m_Entity.m_vec3Velocity.y = 10;
            }
            else if(nUpdateTick > m_nStartTick)
            {
                m_Entity.m_vec3Velocity.y = 0;

                Stop();
            }

//            m_Entity.Jump();
//
//            Stop(); //  next tick stop..? 시작과 같은 틱에 종료..?(괜찮나..? 개념적으로 맞는지 고민해보자)
        }
    }
}