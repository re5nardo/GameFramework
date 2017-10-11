using FlatBuffers;

namespace GameEvent
{
    public class CharacterAttack : IGameEvent
    {
        public int      m_nAttackingEntityID;
        public int      m_nAttackedEntityID;
        public int      m_nDamage;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.CharacterAttack;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.CharacterAttack.GetRootAsCharacterAttack(buf);

            m_nAttackingEntityID = data.AttackingEntityID;
            m_nAttackedEntityID = data.AttackedEntityID;
            m_nDamage = data.Damage;

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, AttackingEntityID : {1}, AttackedEntityID : {2}, Damage : {3}", base.ToString(), m_nAttackingEntityID, m_nAttackedEntityID, m_nDamage);
        }
    }
}
