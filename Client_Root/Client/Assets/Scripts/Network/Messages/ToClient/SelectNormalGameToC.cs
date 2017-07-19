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
        return null;
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.SelectNormalGameToC.GetRootAsSelectNormalGameToC(buf);

        m_nResult = data.Result;
        m_nExpectedTime = data.ExpectedTime;

        return true;
    }
}
