using UnityEngine;
using System.Text;

public class GameEventIdleToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventIdleToR_ID;

    public int m_nPlayerIndex = 0;                          //  json field name : PlayerIndex
    public Vector3 m_vec3Pos = Vector3.zero;                //  Json field name : Pos_X, Pos_Y, Pos_Z

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
        JSONHelper.AddField(jsonObj, "Pos_X", m_vec3Pos.x);
        JSONHelper.AddField(jsonObj, "Pos_Y", m_vec3Pos.y);
        JSONHelper.AddField(jsonObj, "Pos_Z", m_vec3Pos.z);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_X", ref m_vec3Pos.x)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_Y", ref m_vec3Pos.y)) return false;
        if(!JSONHelper.GetField(jsonObj, "Pos_Z", ref m_vec3Pos.z)) return false;

        return true;
    }
}
