using UnityEngine;
using FlatBuffers;

public class GameEventDashToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventDashToR_ID;

    public int m_nPlayerIndex;

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
        FBS.GameEventDashToR.StartGameEventDashToR(m_Builder);
        FBS.GameEventDashToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        var data = FBS.GameEventDashToR.EndGameEventDashToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameEventDashToR.GetRootAsGameEventDashToR(buf);

        m_nPlayerIndex = data.PlayerIndex;

        return true;
    }
}
