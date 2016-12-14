using UnityEngine;
using System.Text;

public class GameEventStopToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventStopToC_ID;

    public int m_nPlayerIndex = 0;                          //  json field name : PlayerIndex
    public long m_lEventTime = 0;                           //  json field name : EventTime
    public Vector3 m_vec3Dest = Vector3.zero;               //  Json field name : Pos_X, Pos_Y, Pos_Z

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
        JSONHelper.AddField(jsonObj, "Pos_X", m_vec3Dest.x);
        JSONHelper.AddField(jsonObj, "Pos_Y", m_vec3Dest.y);
        JSONHelper.AddField(jsonObj, "Pos_Z", m_vec3Dest.z);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "EventTime", ref m_lEventTime)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_X", ref m_vec3Dest.x)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_Y", ref m_vec3Dest.y)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_Z", ref m_vec3Dest.z)) return false;

        return true;
    }
}
