using System.Collections.Generic;
using System.Text;

public class GameStartToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameStartToC_ID;

    public long                 m_lGameElapsedTime;                             //  json field name : GameElapsedTime
    public List<PlayerInfo>     m_listPlayerInfo = new List<PlayerInfo>();      //  json field name : PlayerInfoList
   
    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "GameElapsedTime", m_lGameElapsedTime);
        JSONHelper.AddField<PlayerInfo>(jsonObj, "PlayerInfoList", m_listPlayerInfo);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "GameElapsedTime", ref m_lGameElapsedTime)) return false;
        if(!JSONHelper.GetField(jsonObj, "PlayerInfoList", m_listPlayerInfo)) return false;

        return true;
    }
}
