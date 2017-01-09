using FlatBuffers;

public class PreparationStateToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PreparationStateToC_ID;

    public int m_nPlayerIndex;
    public float m_fState;

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

        PreparationStateToC_Data.StartPreparationStateToC_Data(builder);
        PreparationStateToC_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        PreparationStateToC_Data.AddState(builder, m_fState);
        var data = PreparationStateToC_Data.EndPreparationStateToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = PreparationStateToC_Data.GetRootAsPreparationStateToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_fState = data.State;

        return true;
    }
}
