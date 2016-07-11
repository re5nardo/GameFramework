using UnityEngine;

public class GameEvent_Move_ToS : IGameEvent
{
    public Vector3 m_vec3Dest = Vector3.zero;   //  Json field name : Pos_X, Pos_Y, Pos_Z

    public override ushort GetID()
    {
        return (ushort)Messages.Game_Event_Move_ToS;
    }

    public override string Serialize()
    {
        return GetJSONObject().Print() + '\0';
    }

    public override bool Deserialize(string strJson)
    {
        JSONObject jsonObj = new JSONObject (strJson);

        return SetByJSONObject(jsonObj);
    }

    public override JSONObject GetJSONObject()
    {
        JSONObject jsonObj = base.GetJSONObject();

        jsonObj.AddField ("BasicInfo", base.GetJSONObject());
        jsonObj.AddField ("Pos_X", m_vec3Dest.x);
        jsonObj.AddField ("Pos_Y", m_vec3Dest.y);
        jsonObj.AddField ("Pos_Z", m_vec3Dest.z);

        return jsonObj;
    }

    public override bool SetByJSONObject(JSONObject jsonObj)
    {
        if (!base.SetByJSONObject(jsonObj))
        {
            return false;
        }

        if (jsonObj.HasField ("Pos_X") && jsonObj.GetField ("Pos_X").IsNumber)
        {
            jsonObj.GetField (ref m_vec3Dest.x, "Pos_X");
        } 
        else
        {
            return false;
        }

        if (jsonObj.HasField ("Pos_Y") && jsonObj.GetField ("Pos_Y").IsNumber)
        {
            jsonObj.GetField (ref m_vec3Dest.y, "Pos_Y");
        } 
        else
        {
            return false;
        }

        if (jsonObj.HasField ("Pos_Z") && jsonObj.GetField ("Pos_Z").IsNumber)
        {
            jsonObj.GetField (ref m_vec3Dest.z, "Pos_Z");
        } 
        else
        {
            return false;
        }

        return true;
    }
}
