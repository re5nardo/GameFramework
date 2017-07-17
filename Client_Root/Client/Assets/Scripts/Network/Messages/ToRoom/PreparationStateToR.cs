using FlatBuffers;

public class PreparationStateToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PreparationStateToR_ID;

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
        PreparationStateToR_Data.StartPreparationStateToR_Data(m_Builder);
        PreparationStateToR_Data.AddState(m_Builder, m_fState);
        var data = PreparationStateToR_Data.EndPreparationStateToR_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = PreparationStateToR_Data.GetRootAsPreparationStateToR_Data(buf);

        m_fState = data.State;

        return true;
    }
}
