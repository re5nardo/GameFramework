
public class GameStartToC : IMessage
{
    public long m_lGameElapsedTime;     //  json field name : GameElapsedTime
   
    public ushort GetID()
    {
        return (ushort)Messages.Game_Start_ToC;
    }

    public string Serialize()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("GameElapsedTime", m_lGameElapsedTime);

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

        return true;
    }
}
