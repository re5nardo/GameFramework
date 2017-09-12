using FlatBuffers;

public abstract class IMessage : ISerializable, IDeserializable, IPooledObject
{
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

    public abstract ushort GetID();
    public abstract IMessage Clone();

    public abstract byte[] Serialize();
    public abstract bool Deserialize(byte[] bytes);

    protected FlatBufferBuilder m_Builder = new FlatBufferBuilder(1024);
}
