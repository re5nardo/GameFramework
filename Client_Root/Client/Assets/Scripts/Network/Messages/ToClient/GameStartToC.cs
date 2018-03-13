using FlatBuffers;

public class GameStartToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameStartToC_ID;

    public float m_fTickInterval;
    public int m_nRandomSeed;
    public int m_nTimeLimit;

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
        FBS.GameStartToC.StartGameStartToC(m_Builder);
        FBS.GameStartToC.AddTickInterval(m_Builder, m_fTickInterval);
        FBS.GameStartToC.AddRandomSeed(m_Builder, m_nRandomSeed);
        FBS.GameStartToC.AddTimeLimit(m_Builder, m_nTimeLimit);
        var data = FBS.GameStartToC.EndGameStartToC(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameStartToC.GetRootAsGameStartToC(buf);

        m_fTickInterval = data.TickInterval;
        m_nRandomSeed = data.RandomSeed;
        m_nTimeLimit = data.TimeLimit;

        return true;
    }
}