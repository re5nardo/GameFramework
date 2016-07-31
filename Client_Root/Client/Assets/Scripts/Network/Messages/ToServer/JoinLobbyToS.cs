﻿using System.Text;

public class JoinLobbyToS : IMessage
{
    public ulong    m_nPlayerNumber;        //  json field name : PlayerNumber
    public int      m_nAuthKey;             //  json field name : AuthKey

    public ushort GetID()
    {
        return (ushort)Messages.Join_Lobby_ToS;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerNumber", m_nPlayerNumber);
        JSONHelper.AddField(jsonObj, "AuthKey", m_nAuthKey);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.Default.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerNumber", ref m_nPlayerNumber)) return false;
        if(!JSONHelper.GetField(jsonObj, "AuthKey", ref m_nAuthKey)) return false;

        return true;
    }
}
