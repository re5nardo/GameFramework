using UnityEngine;
using FlatBuffers;

namespace GameEvent
{
    public class EntityDestroy : IGameEvent
    {
        public int m_nEntityID;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.EntityDestroy;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.EntityDestroy.GetRootAsEntityDestroy(buf);

            m_nEntityID = data.EntityID;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}", base.ToString(), m_nEntityID);
        }
    }
}
