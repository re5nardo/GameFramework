using UnityEngine;
using FlatBuffers;

namespace GameEvent
{
    public class Rotation : IGameEvent
    {
        public int      m_nEntityID;
        public float    m_fStartTime;
        public float    m_fEndTime;
        public Vector3  m_vec3StartRotation;
        public Vector3  m_vec3EndRotation;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.Rotation;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.Rotation.GetRootAsRotation(buf);

            m_nEntityID = data.EntityID;
            m_fStartTime = data.StartTime;
            m_fEndTime = data.EndTime;
            m_vec3StartRotation = new Vector3(data.StartRot.X, data.StartRot.Y, data.StartRot.Z);
            m_vec3EndRotation = new Vector3(data.EndRot.X, data.EndRot.Y, data.EndRot.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, StartTime : {2}, EndTime : {3}, StartRotation : {4}, EndRotation : {5}", base.ToString(), m_nEntityID, m_fStartTime, m_fEndTime, m_vec3StartRotation.ToString(), m_vec3EndRotation.ToString());
        }
    }
}