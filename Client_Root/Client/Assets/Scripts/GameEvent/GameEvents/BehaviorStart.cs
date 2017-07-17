using FlatBuffers;

namespace GameEvent
{
    public class BehaviorStart : IGameEvent
    {
        public int      m_nEntityID;
        public float    m_fStartTime;
        public int      m_nBehaviorID;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.BehaviorStart;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.BehaviorStart.GetRootAsBehaviorStart(buf);

            m_nEntityID = data.EntityID;
            m_fStartTime = data.StartTime;
            m_nBehaviorID = data.BehaviorID;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, StartTime : {2}, BehaviorID : {3}", base.ToString(), m_nEntityID, m_fStartTime, m_nBehaviorID);
        }
    }
}
