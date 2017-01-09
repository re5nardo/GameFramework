using UnityEngine;
using FlatBuffers;

public class GameEventMoveToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventMoveToC_ID;

    public int m_nPlayerIndex;
    public long m_lEventTime;
    public Vector3 m_vec3Dest;

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

        var dest = FBSData.Vector3.CreateVector3(builder, m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z);

        GameEventMoveToC_Data.StartGameEventMoveToC_Data(builder);
        GameEventMoveToC_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameEventMoveToC_Data.AddEventTime(builder, m_lEventTime);
        GameEventMoveToC_Data.AddDest(builder, dest);
        var data = GameEventMoveToC_Data.EndGameEventMoveToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameEventMoveToC_Data.GetRootAsGameEventMoveToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_lEventTime = data.EventTime;
        m_vec3Dest = new Vector3(data.Dest.X, data.Dest.Y, data.Dest.Z);

        return true;
    }
}
