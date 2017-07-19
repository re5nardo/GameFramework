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
        FBS.JoinLobbyToC.StartJoinLobbyToC(m_Builder);
        FBS.JoinLobbyToC.AddResult(m_Builder, m_nResult);
        var data = FBS.JoinLobbyToC.EndJoinLobbyToC(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.JoinLobbyToC.GetRootAsJoinLobbyToC(buf);

        m_nResult = data.Result;

        return true;
    }
}
