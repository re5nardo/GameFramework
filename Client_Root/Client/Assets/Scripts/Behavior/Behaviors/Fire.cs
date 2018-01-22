using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class Fire : IBehavior
    {
        IEntity m_Target;

        public override void Initialize(IEntity entity, int nMasterDataID)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;

            MasterData.Behavior masterBehavior = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref masterBehavior);

            m_fLength = masterBehavior.m_fLength;
            m_strStringParams = masterBehavior.m_strStringParams;
            m_listAction = masterBehavior.m_listAction;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
            foreach(MasterData.Behavior.Action action in m_listAction)
            {
                if (nUpdateTick * m_fTickInterval <= action.m_fTime && action.m_fTime < ((nUpdateTick + 1) * m_fTickInterval))
                {
                    if (action.m_strID == "Project")
                    {
                        List<float> listDirection = new List<float>();
                        Util.Parse(m_strStringParams, ',', listDirection);

                        foreach(float dir in listDirection)
                        {
                            int nEntityID = 0;
                            int nProjectileMasterDataID = int.Parse(action.m_listParams[0]);
                            Projectile projectile = null;

                            BaeGameRoom2.Instance.CreateProjectile(nProjectileMasterDataID, ref nEntityID, ref projectile, m_Entity.GetID());
                            projectile.SetPosition(m_Entity.GetPosition());

                            if (m_Entity.GetEntityType() == FBS.Data.EntityType.Character)
                            {
                                Character character = (Character)m_Entity;
                                if (character.GetRole() == Character.Role.Disturber)
                                {
                                    IState state = Factory.Instance.CreateState(projectile, 4);
                                    state.Initialize(projectile, 4);
                                    projectile.AddState(state, nUpdateTick);
                                    state.UpdateTick(nUpdateTick);
                                }
                            }

                            float fTargetAngle = 0;
                            if (m_Target == null)
                            {
                                fTargetAngle = m_Entity.GetRotation().y;
                            }
                            else
                            {
                                fTargetAngle = 0;   //  dummy
                            }

                            projectile.GetBehavior(BehaviorID.MOVE).StartTick(BaeGameRoom2.Instance.GetTickInterval(), nUpdateTick);
                            projectile.GetBehavior(BehaviorID.MOVE).UpdateTick(nUpdateTick);
                        }
                    }
                    else if (action.m_strID == "Fire")
                    {
                    }
                }
            }
        }
    }
}