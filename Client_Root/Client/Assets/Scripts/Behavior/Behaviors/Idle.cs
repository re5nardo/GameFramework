using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Idle : IBehavior
    {
        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
        }
    }
}