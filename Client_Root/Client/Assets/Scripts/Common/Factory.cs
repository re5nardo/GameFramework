using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoSingleton<Factory>
{
    public Entity CreateEntity(FBS.Data.EntityType entityType, int nID, int nMasterDataID)
    {
        GameObject goEntity = new GameObject("Entity_" + nID.ToString());
        Entity entity = goEntity.AddComponent<Entity>();
        entity.Initialize(entityType, nID, nMasterDataID);

        return entity;
    }
}