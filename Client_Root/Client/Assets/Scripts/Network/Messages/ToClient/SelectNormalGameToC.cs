using FlatBuffers;

public class SelectNormalGameToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.SelectNormalGameToC_ID;

    public int m_nResult;
    public int m_nExpectedTime;

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

        SelectNormalGameToC_Data.StartSelectNormalGameToC_Data(builder);
        SelectNormalGameToC_Data.AddResult(builder, m_nResult);
        SelectNormalGameToC_Data.AddExpectedTime(builder, m_nExpectedTime);
        var data = SelectNormalGameToC_Data.EndSelectNormalGameToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = SelectNormalGameToC_Data.GetRootAsSelectNormalGameToC_Data(buf);

        m_nResult = data.Result;
        m_nExpectedTime = data.ExpectedTime;

        return true;
    }
}
