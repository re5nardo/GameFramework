
//	JSON format
public interface IMessage
{
	ushort GetID ();
	string Serialize();
	bool Deserialize(string strJson);
}
