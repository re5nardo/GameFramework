using System.Text;

public class JoinLobbyToC : IMessage
{
    public int m_nResult;     //  json field name : Result

    public ushort GetID()
    {
        return (ushort)Messages.Join_Lobby_ToC;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "Result", m_nResult);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.Default.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "Result", ref m_nResult)) return false;

        return true;
    }
}
