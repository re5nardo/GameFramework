using FlatBuffers;

public abstract class IMessage : ISerializable, IDeserializable
{
    public abstract ushort GetID();
    public abstract IMessage Clone();

    public abstract byte[] Serialize();
    public abstract bool Deserialize(byte[] bytes);

    protected FlatBufferBuilder m_Builder = new FlatBufferBuilder(1024);
}
