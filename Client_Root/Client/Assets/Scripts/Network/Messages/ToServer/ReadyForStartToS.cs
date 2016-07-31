using System.Text;

public class ReadyForStartToS : IMessage
{
    public ulong m_nPlayerNumber;     //  json field name : PlayerNumber

    public ushort GetID()
    {
        return (ushort)Messages.Ready_For_Start_ToS;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerNumber", m_nPlayerNumber);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.Default.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerNumber", ref m_nPlayerNumber)) return false;

        return true;
    }
}
