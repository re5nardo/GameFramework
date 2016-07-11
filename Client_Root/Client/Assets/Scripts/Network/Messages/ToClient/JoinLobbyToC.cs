
public class JoinLobbyToC : IMessage
{
    public int m_nResult;     //  json field name : Result

    public ushort GetID()
    {
        return (ushort)Messages.Join_Lobby_ToC;
    }

    public string Serialize()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("Result", m_nResult);

        return jsonObj.Print () + '\0';
    }

    public bool Deserialize(string strJson)
    {
        JSONObject jsonObj = new JSONObject (strJson);

        if (jsonObj.HasField ("Result") && jsonObj.GetField ("Result").IsNumber)
        {
            jsonObj.GetField (ref m_nResult, "Result");
        } 
        else
        {
            return false;
        }

        return true;
    }
}
