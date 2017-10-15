using UnityEngine;
using FlatBuffers;

public class GameEventRunToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventRunToR_ID;

    public int m_nPlayerIndex;
    public Vector3 m_vec3Dest;

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
        FBS.GameEventRunToR.StartGameEventRunToR(m_Builder);
        FBS.GameEventRunToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        FBS.GameEventRunToR.AddDest(m_Builder, FBS.Data.Vector3.CreateVector3(m_Builder, m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z));
        var data = FBS.GameEventRunToR.EndGameEventRunToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameEventRunToR.GetRootAsGameEventRunToR(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_vec3Dest = new Vector3(data.Dest.X, data.Dest.Y, data.Dest.Z);

        return true;
    }
}
