using UnityEngine;
using FlatBuffers;

namespace GameEvent
{
    public class Collision : IGameEvent
    {
        public int      m_nEntityID;
        public Vector3  m_vec3Position;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.Collision;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.Collision.GetRootAsCollision(buf);

            m_nEntityID = data.EntityID;
            m_vec3Position = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, StartPosition : {2}", base.ToString(), m_nEntityID, m_vec3Position);
        }
    }
}
