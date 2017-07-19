using FlatBuffers;

public class PreparationStateToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PreparationStateToC_ID;

    public int m_nPlayerIndex;
    public float m_fState;

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

        var data = FBS.PreparationStateToC.GetRootAsPreparationStateToC(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_fState = data.State;

        return true;
    }
}
