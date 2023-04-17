using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class Cell
    {
        public HashSet<int> hashEntityId = new HashSet<int>();

        private Vector2Int _position;
        public Vector2Int position
        {
            get => _position;
            private set { _position = value; }
        }

        public Cell(Vector2Int position)
        {
            this.position = position;
        }

        public bool Add(int entityId)
        {
            if (hashEntityId.Contains(entityId))
            {
                Debug.LogWarning("entityId already exists, entityId : " + entityId);
                return false;
            }

            hashEntityId.Add(entityId);

            return true;
        }

        public bool Remove(int entityId)
        {
            if (!hashEntityId.Contains(entityId))
            {
                Debug.LogWarning("entityId doesn't exist, entityId : " + entityId);
                return false;
            }

            hashEntityId.Remove(entityId);

            return true;
        }
    }
}
