using FlatBuffers;

public class EnterRoomToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.EnterRoomToR_ID;

    public string m_strPlayerKey;
    public int m_nAuthKey;
    public int m_nMatchID;

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

        var playerKey = builder.CreateString(m_strPlayerKey);

        EnterRoomToR_Data.StartEnterRoomToR_Data(builder);
        EnterRoomToR_Data.AddPlayerKey(builder, playerKey);
        EnterRoomToR_Data.AddAuthKey(builder, m_nAuthKey);
        EnterRoomToR_Data.AddMatchID(builder, m_nMatchID);
        var data = EnterRoomToR_Data.EndEnterRoomToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = EnterRoomToR_Data.GetRootAsEnterRoomToR_Data(buf);

        m_strPlayerKey = data.PlayerKey;
        m_nAuthKey = data.AuthKey;
        m_nMatchID = data.MatchID;

        return true;
    }
}
