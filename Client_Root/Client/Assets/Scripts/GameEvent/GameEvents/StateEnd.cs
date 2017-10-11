using FlatBuffers;

namespace GameEvent
{
    public class StateEnd : IGameEvent
    {
        public int      m_nEntityID;
        public float    m_fEndTime;
        public int      m_nStateID;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.StateEnd;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.StateEnd.GetRootAsStateEnd(buf);

            m_nEntityID = data.EntityID;
            m_fEndTime = data.EndTime;
            m_nStateID = data.StateID;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, EndTime : {2}, StateID : {3}", base.ToString(), m_nEntityID, m_fEndTime, m_nStateID);
        }
    }
}
