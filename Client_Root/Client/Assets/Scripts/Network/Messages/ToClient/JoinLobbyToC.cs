using FlatBuffers;

public class JoinLobbyToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.JoinLobbyToC_ID;

    public int m_nResult;

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
        JoinLobbyToC_Data.StartJoinLobbyToC_Data(m_Builder);
        JoinLobbyToC_Data.AddResult(m_Builder, m_nResult);
        var data = JoinLobbyToC_Data.EndJoinLobbyToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = JoinLobbyToC_Data.GetRootAsJoinLobbyToC_Data(buf);

        m_nResult = data.Result;

        return true;
    }
}
