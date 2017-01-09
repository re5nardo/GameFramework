using FlatBuffers;

public class GameStartToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameStartToC_ID;

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

        GameStartToC_Data.StartGameStartToC_Data(builder);
        var data = GameStartToC_Data.EndGameStartToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameStartToC_Data.GetRootAsGameStartToC_Data(buf);

        return true;
    }
}
