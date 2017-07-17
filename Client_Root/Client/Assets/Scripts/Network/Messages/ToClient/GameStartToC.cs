using FlatBuffers;

public class GameStartToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameStartToC_ID;

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
        GameStartToC_Data.StartGameStartToC_Data(m_Builder);
        var data = GameStartToC_Data.EndGameStartToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameStartToC_Data.GetRootAsGameStartToC_Data(buf);

        return true;
    }
}
