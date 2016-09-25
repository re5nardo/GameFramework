using System.Text;

public class SelectNormalGameToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.SelectNormalGameToC_ID;

    public int m_nResult;           //  json field name : Result
    public int m_nExpectedTime;     //  json field name : ExpectedTime

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "Result", m_nResult);
        JSONHelper.AddField(jsonObj, "ExpectedTime", m_nExpectedTime);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "Result", ref m_nResult)) return false;
        if(!JSONHelper.GetField(jsonObj, "ExpectedTime", ref m_nExpectedTime)) return false;

        return true;
    }
}
