using System.Text;

public class ReadyForStartToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.ReadyForStartToR_ID;

    public ulong m_nPlayerNumber;     //  json field name : PlayerNumber

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerNumber", m_nPlayerNumber);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerNumber", ref m_nPlayerNumber)) return false;

        return true;
    }
}
