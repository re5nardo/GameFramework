using UnityEngine;
using FlatBuffers;

public class GameEventIdleToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventIdleToR_ID;

    public int m_nPlayerIndex;
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

        GameEventIdleToR_Data.StartGameEventIdleToR_Data(builder);
        GameEventIdleToR_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameEventIdleToR_Data.AddPos(builder, pos);
        var data = GameEventIdleToR_Data.EndGameEventIdleToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameEventIdleToR_Data.GetRootAsGameEventIdleToR_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_vec3Pos = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

        return true;
    }
}
