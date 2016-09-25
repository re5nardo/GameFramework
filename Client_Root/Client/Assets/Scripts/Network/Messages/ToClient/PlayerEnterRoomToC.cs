using System.Text;

public class PlayerEnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PlayerEnterRoomToC_ID;

    public int      m_nPlayerIndex;         //  json field name : PlayerIndex
    public string   m_strCharacterID;       //  json field name : CharacterID

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "PlayerIndex", m_nPlayerIndex);
        JSONHelper.AddField(jsonObj, "CharacterID", m_strCharacterID);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "CharacterID", ref m_strCharacterID)) return false;

        return true;
    }
}
