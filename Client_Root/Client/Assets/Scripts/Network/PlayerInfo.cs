using UnityEngine;
using System.Collections;

public class PlayerInfo : IJSONObjectable
{
    public ulong m_nPlayerNumber = 0;                        //  json field name : PlayerNumber
    public int m_nCharacterNumber = 0;                       //  json field name : CharacterNumber

    public PlayerInfo()
    {
    }

    public PlayerInfo(JSONObject jsonObj)
    {
        SetByJSONObject(jsonObj);
    }

    public JSONObject GetJSONObject()
    {
        JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
        jsonObj.AddField ("PlayerNumber", m_nPlayerNumber.ToString());
        jsonObj.AddField("CharacterNumber", m_nCharacterNumber);

        return jsonObj;
    }

    public bool SetByJSONObject(JSONObject jsonObj)
    {
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

        if (jsonObj.HasField ("CharacterNumber") && jsonObj.GetField ("CharacterNumber").IsNumber)
        {
            jsonObj.GetField (ref m_nCharacterNumber, "CharacterNumber");
        } 
        else
        {
            return false;
        }

        return true;
    }
}