using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Shield : IMagic
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
            m_listAction = masterMagic.m_listAction;
            m_TargetType = masterMagic.m_TargetType;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (m_nStartTick == nUpdateTick)
            {
                Character caster = BaeGameRoom2.Instance.GetCharacter(m_nCasterID);

                IState state = Factory.Instance.CreateState(MasterDataDefine.StateID.SHIELD);
                state.Initialize(caster, MasterDataDefine.StateID.SHIELD, BaeGameRoom2.Instance.GetTickInterval());

                caster.AddState(state, nUpdateTick);

                state.StartTick(nUpdateTick);
                state.UpdateTick(nUpdateTick);
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                BaeGameRoom2.Instance.DestroyMagic(this);
            }
        }
    }
}