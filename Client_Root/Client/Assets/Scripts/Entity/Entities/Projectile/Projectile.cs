using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : IEntity
{
    public override float GetSpeed()
    {
        return 0;
    }

    public override float GetMaximumSpeed()
    {
        return 0;
    }

    public override void Initialize(int nID, int nMasterDataID, params object[] param)
    {
        m_nID = nID;
        m_nMasterDataID = nMasterDataID;

        MasterData.Projectile masterProjectile = null;
        MasterDataManager.Instance.GetData<MasterData.Projectile>(m_nMasterDataID, ref masterProjectile);

        foreach(int nBehaviorID in masterProjectile.m_listBehaviorID)
        {
            m_listBehavior.Add(Factory.Instance.CreateBehavior(this, nBehaviorID));

            m_listBehavior.RemoveAll(x => x == null);
        }

        GameObject goEntityUI = ObjectPool.Instance.GetGameObject("CharacterModel/EntityUI");
        EntityUI entityUI = goEntityUI.GetComponent<EntityUI>();
        entityUI.Initialize(FBS.Data.EntityType.Projectile, nID, nMasterDataID);

        m_EntityUI = entityUI;
    }

    public override FBS.Data.EntityType GetEntityType()
    {
        return FBS.Data.EntityType.Projectile;
    }

    public override void NotifyGameEvent(IGameEvent gameEvent)
    {
    }

    public override void LateUpdateWorld(int nUpdateTick)
    {
        if (m_nDefaultBehaviorID != -1 && !IsBehavioring() && GetBehavior(m_nDefaultBehaviorID) != null)
        {
            GetBehavior(m_nDefaultBehaviorID).StartTick(BaeGameRoom2.Instance.GetTickInterval(), nUpdateTick);
        }

        Move(m_vec3Velocity * BaeGameRoom2.Instance.GetTickInterval());
    }
}
