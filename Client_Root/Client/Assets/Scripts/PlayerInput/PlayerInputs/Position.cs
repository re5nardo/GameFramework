using UnityEngine;
using FlatBuffers;

namespace PlayerInput
{
    public class Position : IPlayerInput
    {
        public int      m_nEntityID;
        public Vector3  m_vec3Position;

        public override FBS.PlayerInputType GetPlayerInputType()
        {
            return FBS.PlayerInputType.Position;
        }

        public override byte[] Serialize()
        {
            var pos = FBS.Data.Vector3.CreateVector3(m_Builder, m_vec3Position.x, m_vec3Position.y, m_vec3Position.z);

            FBS.PlayerInput.Position.StartPosition(m_Builder);
            FBS.PlayerInput.Position.AddEntityID(m_Builder, m_nEntityID);
            FBS.PlayerInput.Position.AddPos(m_Builder, pos);
            var data = FBS.PlayerInput.Position.EndPosition(m_Builder);

            m_Builder.Finish(data.Value);

            return m_Builder.SizedByteArray();
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.PlayerInput.Position.GetRootAsPosition(buf);

            m_nEntityID = data.EntityID;
            m_vec3Position = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("EntityID : {0}, Position : {1}", m_nEntityID, m_vec3Position.ToString());
        }
    }
}