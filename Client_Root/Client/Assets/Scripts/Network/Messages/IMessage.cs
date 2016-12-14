

public interface IMessage : ISerializable, IDeserializable
{
	ushort GetID();
    IMessage Clone();
}
