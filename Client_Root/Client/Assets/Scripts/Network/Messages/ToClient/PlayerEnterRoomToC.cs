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
        return null;
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.PlayerEnterRoomToC.GetRootAsPlayerEnterRoomToC(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_strCharacterID = data.CharacterID;

        return true;
    }
}
