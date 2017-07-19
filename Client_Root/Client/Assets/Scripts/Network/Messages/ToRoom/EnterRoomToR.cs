using FlatBuffers;

public class EnterRoomToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.EnterRoomToR_ID;

    public string m_strPlayerKey;
    public int m_nAuthKey;
    public int m_nMatchID;

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
        var playerKey = m_Builder.CreateString(m_strPlayerKey);

        FBS.EnterRoomToR.StartEnterRoomToR(m_Builder);
        FBS.EnterRoomToR.AddPlayerKey(m_Builder, playerKey);
        FBS.EnterRoomToR.AddAuthKey(m_Builder, m_nAuthKey);
        FBS.EnterRoomToR.AddMatchID(m_Builder, m_nMatchID);
        var data = FBS.EnterRoomToR.EndEnterRoomToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.EnterRoomToR.GetRootAsEnterRoomToR(buf);

        m_strPlayerKey = data.PlayerKey;
        m_nAuthKey = data.AuthKey;
        m_nMatchID = data.MatchID;

        return true;
    }
}
