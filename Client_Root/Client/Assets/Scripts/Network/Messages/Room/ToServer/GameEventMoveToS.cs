using UnityEngine;
using System.Text;

public class GameEventMoveToS : IMessage
{
    public const ushort MESSAGE_ID = 20001;

    public int m_nPlayerIndex = 0;                          //  json field name : PlayerIndex
    public int m_nElapsedTime = 0;                          //  json field name : ElapsedTime
    public Vector3 m_vec3Dest = Vector3.zero;               //  Json field name : Pos_X, Pos_Y, Pos_Z

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerIndex", m_nPlayerIndex);
        JSONHelper.AddField(jsonObj, "ElapsedTime", m_nElapsedTime);
        JSONHelper.AddField(jsonObj, "Pos_X", m_vec3Dest.x);
        JSONHelper.AddField(jsonObj, "Pos_Y", m_vec3Dest.y);
        JSONHelper.AddField(jsonObj, "Pos_Z", m_vec3Dest.z);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "ElapsedTime", ref m_nElapsedTime)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_X", ref m_vec3Dest.x)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_Y", ref m_vec3Dest.y)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_Z", ref m_vec3Dest.z)) return false;

        return true;
    }
}
