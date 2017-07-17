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
        SelectNormalGameToL_Data.StartSelectNormalGameToL_Data(m_Builder);
        var data = SelectNormalGameToL_Data.EndSelectNormalGameToL_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = SelectNormalGameToL_Data.GetRootAsSelectNormalGameToL_Data(buf);

        return true;
    }
}
