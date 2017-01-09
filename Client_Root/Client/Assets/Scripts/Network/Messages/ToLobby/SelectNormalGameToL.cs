using FlatBuffers;

public class SelectNormalGameToL : IMessage
{
    public const ushort MESSAGE_ID = MessageID.SelectNormalGameToL_ID;

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

        SelectNormalGameToL_Data.StartSelectNormalGameToL_Data(builder);
        var data = SelectNormalGameToL_Data.EndSelectNormalGameToL_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = SelectNormalGameToL_Data.GetRootAsSelectNormalGameToL_Data(buf);

        return true;
    }
}
