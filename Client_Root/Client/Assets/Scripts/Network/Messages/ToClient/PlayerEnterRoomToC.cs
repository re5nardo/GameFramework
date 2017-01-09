using FlatBuffers;

public class PlayerEnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PlayerEnterRoomToC_ID;

    public int m_nPlayerIndex;
    public string m_strCharacterID;

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

        var characterID = builder.CreateString(m_strCharacterID);

        PlayerEnterRoomToC_Data.StartPlayerEnterRoomToC_Data(builder);
        PlayerEnterRoomToC_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        PlayerEnterRoomToC_Data.AddCharacterID(builder, characterID);
        var data = PlayerEnterRoomToC_Data.EndPlayerEnterRoomToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = PlayerEnterRoomToC_Data.GetRootAsPlayerEnterRoomToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_strCharacterID = data.CharacterID;

        return true;
    }
}
