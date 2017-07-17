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

        JoinLobbyToL_Data.StartJoinLobbyToL_Data(m_Builder);
        JoinLobbyToL_Data.AddPlayerKey(m_Builder, playerKey);
        JoinLobbyToL_Data.AddAuthKey(m_Builder, m_nAuthKey);
        var data = JoinLobbyToL_Data.EndJoinLobbyToL_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = JoinLobbyToL_Data.GetRootAsJoinLobbyToL_Data(buf);

        m_strPlayerKey = data.PlayerKey;
        m_nAuthKey = data.AuthKey;

        return true;
    }
}
