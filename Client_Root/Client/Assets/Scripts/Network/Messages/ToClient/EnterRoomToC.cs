using System.Text;

public class EnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = 30002;

    public int m_nResult;           //  json field name : Result
    public int m_nPlayerIndex;      //  json field name : PlayerIndex

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "Result", m_nResult);
        JSONHelper.AddField(jsonObj, "PlayerIndex", m_nPlayerIndex);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "Result", ref m_nResult)) return false;
        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;

        return true;
    }
}
