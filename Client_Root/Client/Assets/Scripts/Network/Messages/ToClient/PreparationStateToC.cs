using System.Text;

public class PreparationStateToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PreparationStateToC_ID;

    public int      m_nPlayerIndex;     //  json field name : PlayerIndex
    public float    m_fState;           //  json field name : State

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
        JSONHelper.AddField(jsonObj, "State", m_fState);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "PlayerIndex", ref m_nPlayerIndex)) return false;
        if(!JSONHelper.GetField(jsonObj, "State", ref m_fState)) return false;

        return true;
    }
}
