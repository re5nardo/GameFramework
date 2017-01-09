using FlatBuffers;

public class JoinLobbyToL : IMessage
{
    public const ushort MESSAGE_ID = MessageID.JoinLobbyToL_ID;

    public string m_strPlayerKey;
    public int m_nAuthKey;

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

        JoinLobbyToL_Data.StartJoinLobbyToL_Data(builder);
        JoinLobbyToL_Data.AddPlayerKey(builder, playerKey);
        JoinLobbyToL_Data.AddAuthKey(builder, m_nAuthKey);
        var data = JoinLobbyToL_Data.EndJoinLobbyToL_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = JoinLobbyToL_Data.GetRootAsJoinLobbyToL_Data(buf);

        m_strPlayerKey = data.PlayerKey;
        m_nAuthKey = data.AuthKey;

        return true;
    }
}
