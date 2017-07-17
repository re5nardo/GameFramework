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
        PreparationStateToC_Data.StartPreparationStateToC_Data(m_Builder);
        PreparationStateToC_Data.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        PreparationStateToC_Data.AddState(m_Builder, m_fState);
        var data = PreparationStateToC_Data.EndPreparationStateToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = PreparationStateToC_Data.GetRootAsPreparationStateToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_fState = data.State;

        return true;
    }
}
