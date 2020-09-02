using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameFramework
{
    public class EntityManager : MonoBehaviour
    {
        protected int m_nEntitySequence = 0;
        protected Dictionary<int, IEntity> m_dicEntity = new Dictionary<int, IEntity>();

        protected Grid m_PositionGrid = null;

        public virtual void Clear()
        {
            m_nEntitySequence = 0;
            m_dicEntity.Clear();
        }

        public int GenerateEntityID()
        {
            return m_nEntitySequence++;
        }

        public void RegisterEntity(IEntity entity)
        {
            if (m_dicEntity.ContainsKey(entity.EntityID))
            {
                Debug.LogError("EntityID already exists! EntityID : " + entity.EntityID);
                return;
            }

            m_dicEntity.Add(entity.EntityID, entity);

            m_PositionGrid.Add(entity.EntityID);
        }

        public bool IsRegistered(int nEntityID)
        {
            return m_dicEntity.ContainsKey(nEntityID);
        }

        public void UnregisterEntity(int nEntityID)
        {
            if (!m_dicEntity.ContainsKey(nEntityID))
            {
                Debug.LogError("EntityID does not exist! EntityID : " + nEntityID);
                return;
            }

            m_PositionGrid.Remove(nEntityID);
            m_dicEntity.Remove(nEntityID);
        }

        public IEntity GetEntity(int nEntityID)
        {
            if (m_dicEntity.ContainsKey(nEntityID))
            {
                return m_dicEntity[nEntityID];
            }

            Debug.LogWarning($"There is no entity, nEntityID : {nEntityID}");
            return null;
        }

        public T GetEntity<T>(int nEntityID) where T : IEntity
        {
            if (m_dicEntity.ContainsKey(nEntityID))
            {
                return (T)m_dicEntity[nEntityID];
            }

            Debug.LogWarning($"There is no entity, nEntityID : {nEntityID}");
            return default;
        }

        public List<IEntity> GetEntities(Vector3 vec3Position, float fRadius, List<Predicate<IEntity>> conditions)
        {
            return m_PositionGrid.GetEntities(vec3Position, fRadius, conditions);
        }

        public List<T> GetEntities<T>(Vector3 vec3Position, float fRadius, List<Predicate<IEntity>> conditions) where T : IEntity
        {
            return m_PositionGrid.GetEntities(vec3Position, fRadius, conditions).Cast<T>().ToList();
        }

        public List<IEntity> GetEntities(Transform trTarget, float fFieldOfViewAngle, float fRadius, List<System.Predicate<IEntity>> conditions)
        {
            return m_PositionGrid.GetEntities(trTarget, fFieldOfViewAngle, fRadius, conditions);
        }

        public List<T> GetEntities<T>(Transform trTarget, float fFieldOfViewAngle, float fRadius, List<System.Predicate<IEntity>> conditions) where T : IEntity
        {
            return m_PositionGrid.GetEntities(trTarget, fFieldOfViewAngle, fRadius, conditions).Cast<T>().ToList();
        }

        public List<IEntity> GetAllEntities()
        {
            return new List<IEntity>(m_dicEntity.Values);
        }

        public List<T> GetAllEntities<T>() where T : IEntity
        {
            return m_dicEntity.Values.Cast<T>().ToList();
        }

        public HashSet<int> GetAllEntityIDs()
        {
            return new HashSet<int>(m_dicEntity.Keys);
        }

        public HashSet<Cell> GetCells(Vector2Int vec2Center, float fRadius, bool bMakeCell = false)
        {
            return m_PositionGrid.GetCells(vec2Center, fRadius, bMakeCell);
        }

        public Cell GetCell(Vector2Int vec2CellPosition, bool bMakeCell = false)
        {
            return m_PositionGrid.GetCell(vec2CellPosition, bMakeCell);
        }

        public Vector2Int GetEntityCellPosition(int nEntityID)
        {
            return m_PositionGrid.GetEntityCellPosition(nEntityID);
        }
    }
}
