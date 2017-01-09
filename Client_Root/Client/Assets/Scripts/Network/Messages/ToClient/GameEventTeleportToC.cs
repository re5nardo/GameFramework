using UnityEngine;
using FlatBuffers;

public class GameEventTeleportToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventTeleportToC_ID;

    public int m_nPlayerIndex;
    public long m_lEventTime;
    public Vector3 m_vec3Start;
    public Vector3 m_vec3Dest;
    public int m_nState;

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

        var start = FBSData.Vector3.CreateVector3(builder, m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);
        var dest = FBSData.Vector3.CreateVector3(builder, m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z);

        GameEventTeleportToC_Data.StartGameEventTeleportToC_Data(builder);
        GameEventTeleportToC_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameEventTeleportToC_Data.AddEventTime(builder, m_lEventTime);
        GameEventTeleportToC_Data.AddStart(builder, start);
        GameEventTeleportToC_Data.AddDest(builder, dest);
        GameEventTeleportToC_Data.AddState(builder, m_nState);
        var data = GameEventTeleportToC_Data.EndGameEventTeleportToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameEventTeleportToC_Data.GetRootAsGameEventTeleportToC_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_lEventTime = data.EventTime;
        m_vec3Start = new Vector3(data.Start.X, data.Start.Y, data.Start.Z);
        m_vec3Dest = new Vector3(data.Dest.X, data.Dest.Y, data.Dest.Z);
        m_nState = data.State;

        return true;
    }
}
