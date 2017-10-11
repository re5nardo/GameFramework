using UnityEngine;
using FlatBuffers;

namespace GameEvent
{
    public class CharacterRespawn : IGameEvent
    {
        public int      m_nEntityID;
        public Vector3  m_vec3Position;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.CharacterRespawn;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.CharacterRespawn.GetRootAsCharacterRespawn(buf);

            m_nEntityID = data.EntityID;
            m_vec3Position = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, Position : {2}", base.ToString(), m_nEntityID, m_vec3Position.ToString());
        }
    }
}
