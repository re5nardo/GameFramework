using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public abstract class Grid
    {
        private const int DEFAULT_CELL_SIZE = 10;

        protected Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
        protected Dictionary<int, Vector2Int> entityIDCellPosition = new Dictionary<int, Vector2Int>();

        private int cellSize = DEFAULT_CELL_SIZE;

        public void SetGrid(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public void Clear()
        {
            cells.Clear();
            entityIDCellPosition.Clear();
            cellSize = DEFAULT_CELL_SIZE;
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

            int nRadius = (int)(fRadius / cellSize);

            for (int x = vec2Center.x - nRadius; x <= vec2Center.x + nRadius; ++x)
            {
                for (int z = vec2Center.y - nRadius; z <= vec2Center.y + nRadius; ++z)
                {
                    Vector2Int cellPos = new Vector2Int(x, z);

                    if (bMakeCell && !cells.ContainsKey(cellPos))
                    {
                        cells.Add(cellPos, new Cell(cellPos));
                    }

                    if (cells.ContainsKey(cellPos))
                    {
                        hashCell.Add(cells[cellPos]);
                    }
                }
            }

            return hashCell;
        }

        public Cell GetCell(Vector2Int vec2CellPosition, bool bMakeCell = false)
        {
            if (bMakeCell && !cells.ContainsKey(vec2CellPosition))
            {
                cells.Add(vec2CellPosition, new Cell(vec2CellPosition));
            }

            if (cells.ContainsKey(vec2CellPosition))
            {
                return cells[vec2CellPosition];
            }

            return null;
        }

        public Vector2Int GetCellPosition(Vector3 vec3Position)
        {
            return new Vector2Int((int)(vec3Position.x / cellSize), (int)(vec3Position.z / cellSize));
        }

        public Vector2Int GetEntityCellPosition(int entityId)
        {
            if (!entityIDCellPosition.ContainsKey(entityId))
            {
                Debug.LogWarning("entityId doesn't exist, entityId : " + entityId);
                return Vector2Int.zero;
            }

            return entityIDCellPosition[entityId];
        }
    }
}
