using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameFramework
{
    public class EntityManager : MonoBehaviour
    {
        public const int EMPTY_ENTITY_ID = 0;

        protected int entitySequence = 1;
        protected int localEntitySequence = -1;

        protected Dictionary<int, IEntity> dicEntity = new Dictionary<int, IEntity>();

        protected Grid positionGrid = null;

        public virtual void Clear()
        {
            entitySequence = 1;
            localEntitySequence = -1;
            dicEntity.Clear();
        }

        /// <summary>
        /// Generate (global) EntityId (Range: 1 ~ int.MaxValue)
        /// </summary>
        public int GenerateEntityId()
        {
            return entitySequence++;
        }

        /// <summary>
        /// Generate Local Entityd (Range: -1 ~ int.MinValue)
        /// </summary>
        public int GenerateLocalEntityId()
        {
            return localEntitySequence--;
        }

        public virtual void RegisterEntity(IEntity entity)
        {
            if (dicEntity.ContainsKey(entity.EntityId))
            {
                Debug.LogError("EntityId already exists! EntityId : " + entity.EntityId);
                return;
            }

            dicEntity.Add(entity.EntityId, entity);

            positionGrid.Add(entity.EntityId);
        }

        public bool IsRegistered(int entityId)
        {
            return dicEntity.ContainsKey(entityId);
        }

        public virtual void UnregisterEntity(int entityId)
        {
            if (!dicEntity.ContainsKey(entityId))
            {
                Debug.LogError("EntityId does not exist! EntityId : " + entityId);
                return;
            }

            positionGrid.Remove(entityId);
            dicEntity.Remove(entityId);
        }

        public IEntity GetEntity(int entityId)
        {
            if (dicEntity.TryGetValue(entityId, out IEntity entity))
            {
                return entity;
            }

            Debug.LogWarning($"There is no entity, EntityId : {entityId}");
            return null;
        }

        public T GetEntity<T>(int entityId) where T : IEntity
        {
            if (dicEntity.TryGetValue(entityId, out IEntity entity))
            {
                if (entity is T target)
                {
                    return target;
                }

                return default;
            }

            Debug.LogWarning($"There is no entity, EntityId : {entityId}");
            return default;
        }

        public List<IEntity> GetEntities(Vector3 vec3Position, float fRadius, List<Predicate<IEntity>> conditions)
        {
            return positionGrid.GetEntities(vec3Position, fRadius, conditions);
        }

        public List<T> GetEntities<T>(Vector3 vec3Position, float fRadius, List<Predicate<IEntity>> conditions) where T : IEntity
        {
            return positionGrid.GetEntities(vec3Position, fRadius, conditions).Cast<T>().ToList();
        }

        public List<IEntity> GetEntities(Transform trTarget, float fFieldOfViewAngle, float fRadius, List<System.Predicate<IEntity>> conditions)
        {
            return positionGrid.GetEntities(trTarget, fFieldOfViewAngle, fRadius, conditions);
        }

        public List<T> GetEntities<T>(Transform trTarget, float fFieldOfViewAngle, float fRadius, List<System.Predicate<IEntity>> conditions) where T : IEntity
        {
            return positionGrid.GetEntities(trTarget, fFieldOfViewAngle, fRadius, conditions).Cast<T>().ToList();
        }

        public List<IEntity> GetAllEntities()
        {
            return new List<IEntity>(dicEntity.Values);
        }

        public List<T> GetAllEntities<T>() where T : IEntity
        {
            return new List<T>(dicEntity.Values.Where(x => x is T).Cast<T>());
        }

        public HashSet<int> GetAllEntityIds()
        {
            return new HashSet<int>(dicEntity.Keys);
        }

        public HashSet<Cell> GetCells(Vector2Int vec2Center, float fRadius, bool bMakeCell = false)
        {
            return positionGrid.GetCells(vec2Center, fRadius, bMakeCell);
        }

        public Cell GetCell(Vector2Int vec2CellPosition, bool bMakeCell = false)
        {
            return positionGrid.GetCell(vec2CellPosition, bMakeCell);
        }

        public Vector2Int GetEntityCellPosition(int entityId)
        {
            return positionGrid.GetEntityCellPosition(entityId);
        }
    }
}
