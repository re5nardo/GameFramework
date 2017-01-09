using FlatBuffers;

public class JoinLobbyToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.JoinLobbyToC_ID;

    public int m_nResult;

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

        JoinLobbyToC_Data.StartJoinLobbyToC_Data(builder);
        JoinLobbyToC_Data.AddResult(builder, m_nResult);
        var data = JoinLobbyToC_Data.EndJoinLobbyToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = JoinLobbyToC_Data.GetRootAsJoinLobbyToC_Data(buf);

        m_nResult = data.Result;

        return true;
    }
}
