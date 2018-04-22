using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Thunder : IMagic
    {
        private int m_nTargetID;

        public override void Initialize(int nCasterID, int nID, int nMasterDataID, float fTickInterval)
        {
            m_nCasterID = nCasterID;
            m_nID = nID;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.Magic masterMagic = null;
            MasterDataManager.Instance.GetData<MasterData.Magic>(nMasterDataID, ref masterMagic);

            m_fLength = masterMagic.m_fLength;
            m_listAction = masterMagic.m_listAction;
            m_TargetType = masterMagic.m_TargetType;
        }

        public override void StartTick(int nStartTick, params object[] param)
        {
            base.StartTick(nStartTick, param);

            m_nTargetID = (int)param[0];
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nStartTick == nUpdateTick)
            {
                Character target = BaeGameRoom2.Instance.GetCharacter(m_nTargetID);

                if (!target.IsAlive() || target.HasCoreState(CoreState.CoreState_Invincible))
                    return;

                target.OnAttacked(m_nCasterID, 1, BaeGameRoom2.Instance.GetCurrentTick());
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                BaeGameRoom2.Instance.DestroyMagic(this);
            }
        }
    }
}