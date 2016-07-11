﻿
public class ReadyForStartToS : IMessage
{
    public ulong m_nPlayerNumber;     //  json field name : PlayerNumber

    public ushort GetID()
    {
        return (ushort)Messages.Ready_For_Start_ToS;
    }

    public string Serialize()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("PlayerNumber", m_nPlayerNumber.ToString());

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

        return true;
    }
}
