using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameFramework
{
    public class EntityManager : MonoBehaviour
    {
        protected int entitySequence = 0;
        protected int localEntitySequence = -1;

        protected Dictionary<int, IEntity> dicEntity = new Dictionary<int, IEntity>();

        protected Grid positionGrid = null;

        public virtual void Clear()
        {
            entitySequence = 0;
            dicEntity.Clear();
        }

        /// <summary>
        /// Generate (global) EntityID (Range: 0 ~ int.MaxValue)
        /// </summary>
        public int GenerateEntityID()
        {
            return entitySequence++;
        }

        /// <summary>
        /// Generate Local EntityID (Range: -1 ~ int.MinValue)
        /// </summary>
        public int GenerateLocalEntityID()
        {
            return localEntitySequence--;
        }

        public void RegisterEntity(IEntity entity)
        {
            if (dicEntity.ContainsKey(entity.EntityID))
            {
                Debug.LogError("EntityID already exists! EntityID : " + entity.EntityID);
                return;
            }

            dicEntity.Add(entity.EntityID, entity);

            positionGrid.Add(entity.EntityID);
        }

        public bool IsRegistered(int nEntityID)
        {
            return dicEntity.ContainsKey(nEntityID);
        }

        public void UnregisterEntity(int nEntityID)
        {
            if (!dicEntity.ContainsKey(nEntityID))
            {
                Debug.LogError("EntityID does not exist! EntityID : " + nEntityID);
                return;
            }

            positionGrid.Remove(nEntityID);
            dicEntity.Remove(nEntityID);
        }

        public IEntity GetEntity(int nEntityID)
        {
            if (dicEntity.TryGetValue(nEntityID, out IEntity entity))
            {
                return entity;
            }

            Debug.LogWarning($"There is no entity, nEntityID : {nEntityID}");
            return null;
        }

        public T GetEntity<T>(int nEntityID) where T : IEntity
        {
            if (dicEntity.TryGetValue(nEntityID, out IEntity entity))
            {
                if (entity is T target)
                {
                    return target;
                }

                return default;
            }

            Debug.LogWarning($"There is no entity, nEntityID : {nEntityID}");
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
            return dicEntity.Values.Cast<T>().ToList();
        }

        public HashSet<int> GetAllEntityIDs()
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

        public Vector2Int GetEntityCellPosition(int nEntityID)
        {
            return positionGrid.GetEntityCellPosition(nEntityID);
        }
    }
}
