using System.Collections.Generic;

public class GameStartToC : IMessage
{
    public long                 m_lGameElapsedTime;                             //  json field name : GameElapsedTime
    public List<PlayerInfo>     m_listPlayerInfo = new List<PlayerInfo>();      //  json field name : PlayerInfoList
   
    public ushort GetID()
    {
        return (ushort)Messages.Game_Start_ToC;
    }

    public string Serialize()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("GameElapsedTime", m_lGameElapsedTime);
        JSONHelper.AddField<PlayerInfo>(jsonObj, "PlayerInfoList", m_listPlayerInfo);

        return jsonObj.Print () + '\0';
    }

    public bool Deserialize(string strJson)
    {
        JSONObject jsonObj = new JSONObject (strJson);

        if (jsonObj.HasField ("GameElapsedTime") && jsonObj.GetField ("GameElapsedTime").IsNumber)
        {
            jsonObj.GetField (ref m_lGameElapsedTime, "GameElapsedTime");
        } 
        else
        {
            return false;
        }

        if (jsonObj.HasField ("PlayerInfoList") && jsonObj.GetField ("PlayerInfoList").IsArray)
        {
            m_listPlayerInfo.Clear();

            foreach(JSONObject playerInfo in jsonObj.list)
            {
                m_listPlayerInfo.Add(new PlayerInfo(playerInfo));
            }
        } 
        else
        {
            return false;
        }

        return true;
    }
}
