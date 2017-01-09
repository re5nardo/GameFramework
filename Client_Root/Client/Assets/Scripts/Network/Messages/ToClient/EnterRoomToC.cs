using System.Collections.Generic;
using FlatBuffers;

public class EnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.EnterRoomToC_ID;

    public int m_nResult;
    public int m_nPlayerIndex;
    public Dictionary<int, string> m_dicPlayers = new Dictionary<int, string>();

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
        FlatBufferBuilder builder = new FlatBufferBuilder(1024);

        int[] keys = new int[m_dicPlayers.Keys.Count];
        StringOffset[] values = new StringOffset[m_dicPlayers.Values.Count];

        int nIndex = 0;
        foreach(KeyValuePair<int, string> kv in m_dicPlayers)
        {
            keys[nIndex++] = kv.Key;
            values[nIndex++] = builder.CreateString(kv.Value);
        }

        var playersMapKey = EnterRoomToC_Data.CreatePlayersMapKeyVector(builder, keys);
        var playersMapValue = EnterRoomToC_Data.CreatePlayersMapValueVector(builder, values);

        EnterRoomToC_Data.StartEnterRoomToC_Data(builder);
        EnterRoomToC_Data.AddResult(builder, m_nResult);
        EnterRoomToC_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        EnterRoomToC_Data.AddPlayersMapKey(builder, playersMapKey);
        EnterRoomToC_Data.AddPlayersMapValue(builder, playersMapValue);
        var data = EnterRoomToC_Data.EndEnterRoomToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
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
