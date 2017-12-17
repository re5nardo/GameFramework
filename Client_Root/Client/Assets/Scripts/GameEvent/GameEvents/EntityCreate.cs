using UnityEngine;
using FlatBuffers;

namespace GameEvent
{
    public class EntityCreate : IGameEvent
    {
        public int m_nEntityID;
        public int m_nMasterDataID;
        public FBS.Data.EntityType m_EntityType;
        public Vector3 m_vec3Position;
        public Vector3 m_vec3Rotation;

        public override FBS.GameEventType GetEventType()
        {
            return FBS.GameEventType.EntityCreate;
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.GameEvent.EntityCreate.GetRootAsEntityCreate(buf);

            m_nEntityID = data.EntityID;
            m_nMasterDataID = data.MasterDataID;
            m_EntityType = data.Type;
            m_vec3Position = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);
            m_vec3Rotation = new Vector3(data.Rot.X, data.Rot.Y, data.Rot.Z);

            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}, EntityID : {1}, MasterDataID : {2}, EntityType : {3}, Position : {4}, Rotation : {5}", base.ToString(), m_nEntityID, m_nMasterDataID, m_EntityType, m_vec3Position.ToString(), m_vec3Rotation.ToString());
        }
    }
}
