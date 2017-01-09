using FlatBuffers;

public class PreparationStateToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PreparationStateToR_ID;

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

        PreparationStateToR_Data.StartPreparationStateToR_Data(builder);
        PreparationStateToR_Data.AddState(builder, m_fState);
        var data = PreparationStateToR_Data.EndPreparationStateToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = PreparationStateToR_Data.GetRootAsPreparationStateToR_Data(buf);

        m_fState = data.State;

        return true;
    }
}
