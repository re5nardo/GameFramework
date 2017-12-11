using FlatBuffers;

namespace GameEvent
{
    public class CharacterStatusChange : IGameEvent
    {
        public int      m_nEntityID;
        public string   m_strStatusField;
        public string   m_strReason;
        public float    m_fValue;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.CharacterStatusChange;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.CharacterStatusChange.GetRootAsCharacterStatusChange(buf);

            m_nEntityID = data.EntityID;
            m_strStatusField = data.StatusField;
            m_strReason = data.Reason;
            m_fValue = data.Value;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, StatusField : {2}, Reason : {3}, Value : {4}", base.ToString(), m_nEntityID, m_strStatusField, m_strReason, m_fValue);
        }
    }
}
