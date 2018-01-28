using UnityEngine;
using FlatBuffers;

public class GameEventStopToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventStopToR_ID;

    public int m_nPlayerIndex;
    public Vector3 m_vec3Pos;

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
        FBS.GameEventStopToR.StartGameEventStopToR(m_Builder);
        FBS.GameEventStopToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        FBS.GameEventStopToR.AddPos(m_Builder, FBS.Data.Vector3.CreateVector3(m_Builder, m_vec3Pos.x, m_vec3Pos.y, m_vec3Pos.z));
        var data = FBS.GameEventStopToR.EndGameEventStopToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameEventStopToR.GetRootAsGameEventStopToR(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_vec3Pos = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

        return true;
    }
}
