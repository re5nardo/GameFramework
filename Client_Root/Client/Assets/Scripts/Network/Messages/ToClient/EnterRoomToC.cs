using System.Collections.Generic;
using FlatBuffers;

public class EnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.EnterRoomToC_ID;

    public int m_nResult;
    public int m_nPlayerIndex;
    public Dictionary<int, string> m_dicPlayers = new Dictionary<int, string>();

    public override ushort GetID()
    {
        return MESSAGE_ID;
    }

    public override IMessage Clone()
    {
        return null; 
    }

    public override byte[] Serialize()
    {
        int[] keys = new int[m_dicPlayers.Keys.Count];
        StringOffset[] values = new StringOffset[m_dicPlayers.Values.Count];

        int nIndex = 0;
        foreach(KeyValuePair<int, string> kv in m_dicPlayers)
        {
            keys[nIndex++] = kv.Key;
            values[nIndex++] = m_Builder.CreateString(kv.Value);
        }

        var playersMapKey = EnterRoomToC_Data.CreatePlayersMapKeyVector(m_Builder, keys);
        var playersMapValue = EnterRoomToC_Data.CreatePlayersMapValueVector(m_Builder, values);

        EnterRoomToC_Data.StartEnterRoomToC_Data(m_Builder);
        EnterRoomToC_Data.AddResult(m_Builder, m_nResult);
        EnterRoomToC_Data.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        EnterRoomToC_Data.AddPlayersMapKey(m_Builder, playersMapKey);
        EnterRoomToC_Data.AddPlayersMapValue(m_Builder, playersMapValue);
        var data = EnterRoomToC_Data.EndEnterRoomToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = EnterRoomToC_Data.GetRootAsEnterRoomToC_Data(buf);

        m_nResult = data.Result;
        m_nPlayerIndex = data.PlayerIndex;
        for (int i = 0; i < data.PlayersMapKeyLength; ++i)
        {
            m_dicPlayers.Add(data.GetPlayersMapKey(i), data.GetPlayersMapValue(i));
        }

        return true;
    }
}
