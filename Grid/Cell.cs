using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class Cell
    {
        public HashSet<int> m_hashEntityID = new HashSet<int>();

        private Vector2Int m_vec2Position__;
        public Vector2Int m_vec2Position
        {
            get { return m_vec2Position__; }
            private set { m_vec2Position__ = value; }
        }

        public Cell(Vector2Int vec2Position)
        {
            m_vec2Position = vec2Position;
        }

        public bool Add(int nEntityID)
        {
            if (m_hashEntityID.Contains(nEntityID))
            {
                Debug.LogWarning("nEntityID already exists, nEntityID : " + nEntityID);
                return false;
            }

            m_hashEntityID.Add(nEntityID);

            return true;
        }

        public bool Remove(int nEntityID)
        {
            if (!m_hashEntityID.Contains(nEntityID))
            {
                Debug.LogWarning("nEntityID doesn't exist, nEntityID : " + nEntityID);
                return false;
            }

            m_hashEntityID.Remove(nEntityID);

            return true;
        }
    }
}
