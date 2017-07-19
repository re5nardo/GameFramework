using FlatBuffers;

public class ReadyForStartToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.ReadyForStartToR_ID;

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
        FBS.ReadyForStartToR.StartReadyForStartToR(m_Builder);
        FBS.ReadyForStartToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        var data = FBS.ReadyForStartToR.EndReadyForStartToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.ReadyForStartToR.GetRootAsReadyForStartToR(buf);

        m_nPlayerIndex = data.PlayerIndex;

        return true;
    }
}
