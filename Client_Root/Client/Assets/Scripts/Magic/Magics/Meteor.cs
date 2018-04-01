using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Meteor : IMagic
    {
        public override void Initialize(int nCasterID, int nMasterDataID, float fTickInterval)
        {
            m_nCasterID = nCasterID;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;
        }

        protected override void UpdateBody(int nUpdateTick)
        {

        }
    }
}