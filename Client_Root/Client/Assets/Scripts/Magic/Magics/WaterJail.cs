using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class WaterJail : IMagic
    {
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
            Debug.Log("WaterJail");

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                BaeGameRoom2.Instance.DestroyMagic(this);
            }
        }
    }
}