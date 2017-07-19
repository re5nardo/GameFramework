using FlatBuffers;

public class SelectNormalGameToL : IMessage
{
    public const ushort MESSAGE_ID = MessageID.SelectNormalGameToL_ID;

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
        FBS.SelectNormalGameToL.StartSelectNormalGameToL(m_Builder);
        var data = FBS.SelectNormalGameToL.EndSelectNormalGameToL(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.SelectNormalGameToL.GetRootAsSelectNormalGameToL(buf);

        return true;
    }
}
