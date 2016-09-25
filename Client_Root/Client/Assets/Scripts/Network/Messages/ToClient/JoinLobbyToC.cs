using System.Text;

public class JoinLobbyToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.JoinLobbyToC_ID;

    public int m_nResult;     //  json field name : Result

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "Result", m_nResult);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "Result", ref m_nResult)) return false;

        return true;
    }
}
