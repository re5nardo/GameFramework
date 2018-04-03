using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Boost : IMagic
    {
        public override void Initialize(int nCasterID, int nMasterDataID, float fTickInterval)
        {
            m_nCasterID = nCasterID;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.Magic masterMagic = null;
            MasterDataManager.Instance.GetData<MasterData.Magic>(nMasterDataID, ref masterMagic);

            m_fLength = masterMagic.m_fLength;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            Debug.Log("Boost");

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                BaeGameRoom2.Instance.DestroyMagic(this);
            }
        }
    }
}