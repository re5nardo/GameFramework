using FlatBuffers;

public class GameInputMoveToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameInputMoveToR_ID;

    public int m_nPlayerIndex;
    public FBS.MoveDirection m_Direction;

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
        FBS.GameInputMoveToR.StartGameInputMoveToR(m_Builder);
        FBS.GameInputMoveToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        FBS.GameInputMoveToR.AddDirection(m_Builder, m_Direction);
        var data = FBS.GameInputMoveToR.EndGameInputMoveToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameInputMoveToR.GetRootAsGameInputMoveToR(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_Direction = data.Direction;

        return true;
    }
}
