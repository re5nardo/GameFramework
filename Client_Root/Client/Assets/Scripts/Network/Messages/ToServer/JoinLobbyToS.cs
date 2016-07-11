
public class JoinLobbyToS : IMessage
{
    public ulong    m_nPlayerNumber;        //  json field name : PlayerNumber
    public int      m_nAuthKey;             //  json field name : AuthKey

    public ushort GetID()
    {
        return (ushort)Messages.Join_Lobby_ToS;
    }

    public string Serialize()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("PlayerNumber", m_nPlayerNumber.ToString());
        jsonObj.AddField ("AuthKey", m_nAuthKey);

        return jsonObj.Print () + '\0';
    }

    public bool Deserialize(string strJson)
    {
        JSONObject jsonObj = new JSONObject (strJson);

        if (jsonObj.HasField ("PlayerNumber") && jsonObj.GetField ("PlayerNumber").IsString)
        {
            string strPlayerNumber = "";
            jsonObj.GetField (ref strPlayerNumber, "PlayerIndex");

            m_nPlayerNumber = uint.Parse(strPlayerNumber);
        } 
        else
        {
            return false;
        }

        if (jsonObj.HasField ("AuthKey") && jsonObj.GetField ("AuthKey").IsNumber)
        {
            jsonObj.GetField (ref m_nAuthKey, "AuthKey");
        } 
        else
        {
            return false;
        }

        return true;
    }
}
