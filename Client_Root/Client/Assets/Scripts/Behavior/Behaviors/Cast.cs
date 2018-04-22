using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Cast : IBehavior
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
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            foreach (MasterData.Behavior.Action action in m_listAction)
            {
                int nTick = (int)(action.m_fTime / m_fTickInterval);

                if (nUpdateTick == nTick + m_nStartTick)
                {
                    if (action.m_strID == "Cast")
                    {
                        int nMagicID = 0;
                        Util.Convert(action.m_listParams[0], ref nMagicID);

                        int nEntityID = 0;
                        IMagic magic = null;
                        BaeGameRoom2.Instance.CreateMagic(nMagicID, ref nEntityID, ref magic, m_Entity.GetID());

                        if (magic.GetTargetType() == Magic.TargetType.JustHigher)
                        {
                            int nTargetID = BaeGameRoom2.Instance.GetJustHigherRankPlayerEntityID(m_Entity.GetID());

                            if (nTargetID == -1)
                            {
                                BaeGameRoom2.Instance.DestroyMagic(magic);
                                return;
                            }
                            
                            magic.StartTick(nUpdateTick, nTargetID);
                            magic.UpdateTick(nUpdateTick);
                        }
                        else
                        {
                            magic.StartTick(nUpdateTick);
                            magic.UpdateTick(nUpdateTick);
                        }
                    }
                }
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                Stop();
            }
        }
    }
}