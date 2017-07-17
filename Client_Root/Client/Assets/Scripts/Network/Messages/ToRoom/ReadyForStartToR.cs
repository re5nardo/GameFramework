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
        ReadyForStartToR_Data.StartReadyForStartToR_Data(m_Builder);
        ReadyForStartToR_Data.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        var data = ReadyForStartToR_Data.EndReadyForStartToR_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = ReadyForStartToR_Data.GetRootAsReadyForStartToR_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;

        return true;
    }
}
