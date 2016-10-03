using System.Text;
using System.Collections.Generic;

public class EnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.EnterRoomToC_ID;

    public int m_nResult;                                                           //  json field name : Result
    public int m_nPlayerIndex;                                                      //  json field name : PlayerIndex
    public Dictionary<int, string> m_dicPlayers = new Dictionary<int, string>();    //  json field name : Players

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "Result", m_nResult);
        JSONHelper.AddField(jsonObj, "PlayerIndex", m_nPlayerIndex);
        JSONHelper.AddField(jsonObj, "Players", m_dicPlayers);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "Result", ref m_nResult)) return false;
        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "Players", m_dicPlayers)) return false;

        return true;
    }
}
