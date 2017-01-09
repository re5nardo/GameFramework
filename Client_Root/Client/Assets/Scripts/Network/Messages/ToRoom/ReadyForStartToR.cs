using FlatBuffers;

public class ReadyForStartToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.ReadyForStartToR_ID;

    public int m_nPlayerIndex;

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public IMessage Clone()
    {
        return null; 
    }

    public byte[] Serialize()
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1024);

        ReadyForStartToR_Data.StartReadyForStartToR_Data(builder);
        ReadyForStartToR_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        var data = ReadyForStartToR_Data.EndReadyForStartToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = ReadyForStartToR_Data.GetRootAsReadyForStartToR_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;

        return true;
    }
}
