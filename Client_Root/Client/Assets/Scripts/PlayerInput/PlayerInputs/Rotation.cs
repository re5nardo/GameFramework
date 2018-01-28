using UnityEngine;
using FlatBuffers;

namespace PlayerInput
{
    public class Rotation : IPlayerInput
    {
        public int      m_nEntityID;
        public Vector3  m_vec3Rotation;

        public override FBS.PlayerInputType GetPlayerInputType()
        {
            return FBS.PlayerInputType.Rotation;
        }

        public override byte[] Serialize()
        {
            FBS.PlayerInput.Rotation.StartRotation(m_Builder);
            FBS.PlayerInput.Rotation.AddEntityID(m_Builder, m_nEntityID);
            FBS.PlayerInput.Rotation.AddRot(m_Builder, FBS.Data.Vector3.CreateVector3(m_Builder, m_vec3Rotation.x, m_vec3Rotation.y, m_vec3Rotation.z));
            var data = FBS.PlayerInput.Rotation.EndRotation(m_Builder);

            m_Builder.Finish(data.Value);

            return m_Builder.SizedByteArray();
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.PlayerInput.Rotation.GetRootAsRotation(buf);

            m_nEntityID = data.EntityID;
            m_vec3Rotation = new Vector3(data.Rot.X, data.Rot.Y, data.Rot.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("EntityID : {0}, Rotation : {1}", m_nEntityID, m_vec3Rotation.ToString());
        }
    }
}