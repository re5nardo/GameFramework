using FlatBuffers;

public abstract class IPlayerInput : ISerializable, IDeserializable, IPooledObject
{
    protected FlatBufferBuilder m_Builder = new FlatBufferBuilder(1024);

    protected DestroyType m_DestroyType_ = DestroyType.Normal;
    public DestroyType m_DestroyType
    {
        get
        {
            return m_DestroyType_;
        }
        set
        {
            m_DestroyType_ = value;
        }
    }

    public virtual void OnUsed()
    {
    }

    public virtual void OnReturned()
    {
    }

    public abstract FBS.PlayerInputType GetPlayerInputType();
    public abstract byte[] Serialize();
    public abstract bool Deserialize(byte[] bytes);
}