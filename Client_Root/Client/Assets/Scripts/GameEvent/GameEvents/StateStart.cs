using FlatBuffers;

namespace GameEvent
{
    public class StateStart : IGameEvent
    {
        public int      m_nEntityID;
        public float    m_fStartTime;
        public int      m_nStateID;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.StateStart;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.StateStart.GetRootAsStateStart(buf);

            m_nEntityID = data.EntityID;
            m_fStartTime = data.StartTime;
            m_nStateID = data.StateID;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, StartTime : {2}, StateID : {3}", base.ToString(), m_nEntityID, m_fStartTime, m_nStateID);
        }
    }
}
