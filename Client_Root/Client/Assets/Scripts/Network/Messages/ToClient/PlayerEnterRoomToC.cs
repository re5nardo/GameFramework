using FlatBuffers;

public class PlayerEnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PlayerEnterRoomToC_ID;

    public int m_nPlayerIndex;
    public string m_strCharacterID;

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
        var characterID = m_Builder.CreateString(m_strCharacterID);

        PlayerEnterRoomToC_Data.StartPlayerEnterRoomToC_Data(m_Builder);
        PlayerEnterRoomToC_Data.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        PlayerEnterRoomToC_Data.AddCharacterID(m_Builder, characterID);
        var data = PlayerEnterRoomToC_Data.EndPlayerEnterRoomToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = PlayerEnterRoomToC_Data.GetRootAsPlayerEnterRoomToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_strCharacterID = data.CharacterID;

        return true;
    }
}
