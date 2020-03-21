using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public abstract class Grid
    {
        private Dictionary<Vector2Int, Cell> m_Cells = new Dictionary<Vector2Int, Cell>();
        private Dictionary<int, Vector2Int> m_EntityIDCellPosition = new Dictionary<int, Vector2Int>();

        private int m_nCellSize = 10;

        public void SetGrid(int nCellSize)
        {
            m_nCellSize = nCellSize;
        }

        public void Clear()
        {
            m_Cells.Clear();
            m_EntityIDCellPosition.Clear();
            m_nCellSize = 10;
        }

        public abstract void Add(int nEntityID, bool bPublish = true);

        public abstract void Remove(int nEntityID, bool bPublish = true);

        public abstract void Move(int nEntityID);

        public abstract List<IEntity> GetEntities(Vector3 vec3Position, float fRadius, List<Predicate<IEntity>> conditions);

        public abstract List<IEntity> GetEntities(Transform trTarget, float fFieldOfViewAngle, float fRadius, List<Predicate<IEntity>> conditions);

        public abstract List<IEntity> GetEntities(Vector2Int vec2CellPos, List<Predicate<IEntity>> conditions);

        public HashSet<Cell> GetCells(Vector2Int vec2Center, float fRadius, bool bMakeCell = false)
        {
            HashSet<Cell> hashCell = new HashSet<Cell>();

            int nRadius = (int)(fRadius / m_nCellSize);

            for (int x = vec2Center.x - nRadius; x <= vec2Center.x + nRadius; ++x)
            {
                for (int z = vec2Center.y - nRadius; z <= vec2Center.y + nRadius; ++z)
                {
                    Vector2Int cellPos = new Vector2Int(x, z);

                    if (bMakeCell && !m_Cells.ContainsKey(cellPos))
                    {
                        m_Cells.Add(cellPos, new Cell(cellPos));
                    }

                    if (m_Cells.ContainsKey(cellPos))
                    {
                        hashCell.Add(m_Cells[cellPos]);
                    }
                }
            }

            return hashCell;
        }

        public Cell GetCell(Vector2Int vec2CellPosition, bool bMakeCell = false)
        {
            if (bMakeCell && !m_Cells.ContainsKey(vec2CellPosition))
            {
                m_Cells.Add(vec2CellPosition, new Cell(vec2CellPosition));
            }

            if (m_Cells.ContainsKey(vec2CellPosition))
            {
                return m_Cells[vec2CellPosition];
            }

            return null;
        }

        public Vector2Int GetCellPosition(Vector3 vec3Position)
        {
            return new Vector2Int((int)(vec3Position.x / m_nCellSize), (int)(vec3Position.z / m_nCellSize));
        }

        public Vector2Int GetEntityCellPosition(int nEntityID)
        {
            if (!m_EntityIDCellPosition.ContainsKey(nEntityID))
            {
                Debug.LogWarning("nEntityID doesn't exist, nEntityID : " + nEntityID);
                return Vector2Int.zero;
            }

            return m_EntityIDCellPosition[nEntityID];
        }
    }
}
