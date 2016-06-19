
public class ReadyForStartToS : IMessage
{
    public int m_nPlayerIndex;     //  json field name : PlayerIndex

    public ushort GetID()
    {
        return (ushort)Messages.Ready_For_Start_ToS;
    }

    public string Serialize()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("PlayerIndex", m_nPlayerIndex);

        return jsonObj.Print () + '\0';
    }

    public bool Deserialize(string strJson)
    {
        JSONObject jsonObj = new JSONObject (strJson);

        if (jsonObj.HasField ("PlayerIndex") && jsonObj.GetField ("PlayerIndex").IsNumber)
        {
            jsonObj.GetField (ref m_nPlayerIndex, "PlayerIndex");
        } 
        else
        {
            return false;
        }

        return true;
    }
}
