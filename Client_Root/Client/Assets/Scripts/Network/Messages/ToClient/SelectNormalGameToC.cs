using FlatBuffers;

public class SelectNormalGameToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.SelectNormalGameToC_ID;

    public int m_nResult;
    public int m_nExpectedTime;

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
        SelectNormalGameToC_Data.StartSelectNormalGameToC_Data(m_Builder);
        SelectNormalGameToC_Data.AddResult(m_Builder, m_nResult);
        SelectNormalGameToC_Data.AddExpectedTime(m_Builder, m_nExpectedTime);
        var data = SelectNormalGameToC_Data.EndSelectNormalGameToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = SelectNormalGameToC_Data.GetRootAsSelectNormalGameToC_Data(buf);

        m_nResult = data.Result;
        m_nExpectedTime = data.ExpectedTime;

        return true;
    }
}
