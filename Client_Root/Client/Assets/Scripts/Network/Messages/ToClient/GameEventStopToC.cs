using UnityEngine;
using FlatBuffers;

public class GameEventStopToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventStopToC_ID;

    public int m_nPlayerIndex;
    public long m_lEventTime;
    public Vector3 m_vec3Pos;

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

        var pos = FBSData.Vector3.CreateVector3(builder, m_vec3Pos.x, m_vec3Pos.y, m_vec3Pos.z);

        GameEventStopToC_Data.StartGameEventStopToC_Data(builder);
        GameEventStopToC_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameEventStopToC_Data.AddEventTime(builder, m_lEventTime);
        GameEventStopToC_Data.AddPos(builder, pos);
        var data = GameEventStopToC_Data.EndGameEventStopToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameEventStopToC_Data.GetRootAsGameEventStopToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_lEventTime = data.EventTime;
        m_vec3Pos = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

        return true;
    }
}
