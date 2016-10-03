using System.Text;

public class PreparationStateToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PreparationStateToR_ID;

    public float    m_fState;           //  json field name : State

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public byte[] Serialize()
    {
        JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

        JSONHelper.AddField(jsonObj, "State", m_fState);

        return Encoding.Default.GetBytes(jsonObj.Print());
    }

    public bool Deserialize(byte[] bytes)
    {
        JSONObject jsonObj = new JSONObject(Encoding.UTF8.GetString(bytes));

        if(!JSONHelper.GetField(jsonObj, "State", ref m_fState)) return false;

        return true;
    }
}
