using UnityEngine;
using System.Text;

public class GameEventTeleportToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventTeleportToC_ID;

    public int m_nPlayerIndex = 0;                          //  json field name : PlayerIndex
    public long m_lEventTime = 0;                           //  json field name : EventTime
    public Vector3 m_vec3Start = Vector3.zero;              //  Json field name : Start_X, Start_Y, Start_Z
    public Vector3 m_vec3Dest = Vector3.zero;               //  Json field name : Dest_X, Dest_Y, Dest_Z
    public int m_nState = 0;                                //  Json field name : State

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public IMessage Clone()
    {
        return null; 
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerIndex", m_nPlayerIndex);
        JSONHelper.AddField(jsonObj, "EventTime", m_lEventTime);
        JSONHelper.AddField(jsonObj, "Start_X", m_vec3Start.x);
        JSONHelper.AddField(jsonObj, "Start_Y", m_vec3Start.y);
        JSONHelper.AddField(jsonObj, "Start_Z", m_vec3Start.z);
        JSONHelper.AddField(jsonObj, "Dest_X", m_vec3Dest.x);
        JSONHelper.AddField(jsonObj, "Dest_Y", m_vec3Dest.y);
        JSONHelper.AddField(jsonObj, "Dest_Z", m_vec3Dest.z);
        JSONHelper.AddField(jsonObj, "State", m_nState);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "EventTime", ref m_lEventTime)) return false;
        if(!JSONHelper.GetField(jsonObj, "Start_X", ref m_vec3Start.x)) return false;
        if(!JSONHelper.GetField(jsonObj, "Start_Y", ref m_vec3Start.y)) return false;
        if(!JSONHelper.GetField(jsonObj, "Start_Z", ref m_vec3Start.z)) return false;
        if(!JSONHelper.GetField(jsonObj, "Dest_X", ref m_vec3Dest.x)) return false;
        if(!JSONHelper.GetField(jsonObj, "Dest_Y", ref m_vec3Dest.y)) return false;
        if(!JSONHelper.GetField(jsonObj, "Dest_Z", ref m_vec3Dest.z)) return false;
        if(!JSONHelper.GetField(jsonObj, "State", ref m_nState)) return false;

        return true;
    }
}
