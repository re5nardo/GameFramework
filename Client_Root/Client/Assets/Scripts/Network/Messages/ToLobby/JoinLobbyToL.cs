using System.Text;

public class JoinLobbyToL : IMessage
{
    public const ushort MESSAGE_ID = MessageID.JoinLobbyToL_ID;

    public string   m_strPlayerKey;         //  json field name : PlayerKey
    public int      m_nAuthKey;             //  json field name : AuthKey

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public IMessage Clone()
    {
        return null; 
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerKey", m_strPlayerKey);
        JSONHelper.AddField(jsonObj, "AuthKey", m_nAuthKey);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerNumber", ref m_strPlayerKey)) return false;
        if(!JSONHelper.GetField(jsonObj, "AuthKey", ref m_nAuthKey)) return false;

        return true;
    }
}
