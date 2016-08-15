using System.Text;

public class SelectNormalGameToS : IMessage
{
    public ushort GetID()
    {
        return (ushort)Messages.SelectNormalGameToS_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        return true;
    }
}
