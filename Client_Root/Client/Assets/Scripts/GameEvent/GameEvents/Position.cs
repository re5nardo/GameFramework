using UnityEngine;
using FlatBuffers;

namespace GameEvent
{
    public class Position : IGameEvent
    {
        public int      m_nEntityID;
        public float    m_fStartTime;
        public float    m_fEndTime;
        public Vector3  m_vec3StartPosition;
        public Vector3  m_vec3EndPosition;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.Position;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.Position.GetRootAsPosition(buf);

            m_nEntityID = data.EntityID;
            m_fStartTime = data.StartTime;
            m_fEndTime = data.EndTime;
            m_vec3StartPosition = new Vector3(data.StartPos.X, data.StartPos.Y, data.StartPos.Z);
            m_vec3EndPosition = new Vector3(data.EndPos.X, data.EndPos.Y, data.EndPos.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, StartTime : {2}, EndTime : {3}, StartPosition : {4}, EndPosition : {5}", base.ToString(), m_nEntityID, m_fStartTime, m_fEndTime, m_vec3StartPosition.ToString(), m_vec3EndPosition.ToString());
        }
    }
}
