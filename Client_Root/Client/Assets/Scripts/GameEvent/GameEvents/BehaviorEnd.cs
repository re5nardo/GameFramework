using FlatBuffers;

namespace GameEvent
{
    public class BehaviorEnd : IGameEvent
    {
        public int      m_nEntityID;
        public float    m_fEndTime;
        public int      m_nBehaviorID;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.BehaviorEnd;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.BehaviorEnd.GetRootAsBehaviorEnd(buf);

            m_nEntityID = data.EntityID;
            m_fEndTime = data.EndTime;
            m_nBehaviorID = data.BehaviorID;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, EndTime : {2}, BehaviorID : {3}", base.ToString(), m_nEntityID, m_fEndTime, m_nBehaviorID);
        }
    }
}