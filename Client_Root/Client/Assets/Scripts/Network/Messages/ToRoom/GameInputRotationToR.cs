using UnityEngine;
using FlatBuffers;

public class GameInputRotationToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameInputRotationToR_ID;

    public int m_nPlayerIndex;
    public Vector3 m_Rotation;

    public override ushort GetID()
    {
        return MESSAGE_ID;
    }

    public override IMessage Clone()
    {
        return null; 
    }

    public override byte[] Serialize()
    {
        FBS.GameInputRotationToR.StartGameInputRotationToR(m_Builder);
        FBS.GameInputRotationToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        FBS.GameInputRotationToR.AddRotation(m_Builder, FBS.Data.Vector3.CreateVector3(m_Builder, m_Rotation.x, m_Rotation.y, m_Rotation.z));
        var data = FBS.GameInputRotationToR.EndGameInputRotationToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameInputRotationToR.GetRootAsGameInputRotationToR(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_Rotation = new Vector3(data.Rotation.X, data.Rotation.Y, data.Rotation.Z);

        return true;
    }
}
