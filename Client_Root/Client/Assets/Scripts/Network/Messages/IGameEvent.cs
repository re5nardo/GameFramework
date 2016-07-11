
public abstract class IGameEvent : IMessage, IJSONObjectable
{
    public int m_nPlayerIndex = 0;                          //  json field name : PlayerIndex
    public int m_nElapsedTime = 0;                          //  json field name : ElapsedTime

    public virtual JSONObject GetJSONObject()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("PlayerIndex", m_nPlayerIndex);
        jsonObj.AddField("ElapsedTime", m_nElapsedTime);

        return jsonObj;
    }

    public virtual bool SetByJSONObject(JSONObject jsonObj)
    {
        if (jsonObj.HasField ("PlayerIndex") && jsonObj.GetField ("PlayerIndex").IsNumber)
        {
            jsonObj.GetField (ref m_nPlayerIndex, "PlayerIndex");
        } 
        else
        {
            return false;
        }

        if (jsonObj.HasField ("ElapsedTime") && jsonObj.GetField ("ElapsedTime").IsNumber)
        {
            jsonObj.GetField (ref m_nElapsedTime, "ElapsedTime");
        } 
        else
        {
            return false;
        }

        return true;
    }

    public abstract ushort GetID ();
    public abstract string Serialize();
    public abstract bool Deserialize(string strJson);
}