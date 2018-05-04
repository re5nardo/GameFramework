using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Jump : IBehavior
    {
        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.Behavior masterBehavior = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref masterBehavior);

            m_fLength = masterBehavior.m_fLength;
            m_strStringParams = masterBehavior.m_strStringParams;
            m_listAction = masterBehavior.m_listAction;

			m_bPredictPlay = true;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            if (nUpdateTick == m_nStartTick)
            {
                Character character = m_Entity as Character;

                character.Jump();

                Stop();
            }

//            Stop(); //  next tick stop..? 시작과 같은 틱에 종료..?(괜찮나..? 개념적으로 맞는지 고민해보자)
        }
    }
}