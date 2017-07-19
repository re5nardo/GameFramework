using FlatBuffers;

public class JoinLobbyToL : IMessage
{
    public const ushort MESSAGE_ID = MessageID.JoinLobbyToL_ID;

    public string m_strPlayerKey;
    public int m_nAuthKey;

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

        FBS.JoinLobbyToL.StartJoinLobbyToL(m_Builder);
        FBS.JoinLobbyToL.AddPlayerKey(m_Builder, playerKey);
        FBS.JoinLobbyToL.AddAuthKey(m_Builder, m_nAuthKey);
        var data = FBS.JoinLobbyToL.EndJoinLobbyToL(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.JoinLobbyToL.GetRootAsJoinLobbyToL(buf);

        m_strPlayerKey = data.PlayerKey;
        m_nAuthKey = data.AuthKey;

        return true;
    }
}
