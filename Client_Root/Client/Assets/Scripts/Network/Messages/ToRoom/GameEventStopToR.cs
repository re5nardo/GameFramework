using UnityEngine;
using FlatBuffers;

public class GameEventStopToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventStopToR_ID;

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

        GameEventStopToR_Data.StartGameEventStopToR_Data(builder);
        GameEventStopToR_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameEventStopToR_Data.AddPos(builder, pos);
        var data = GameEventStopToR_Data.EndGameEventStopToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameEventStopToR_Data.GetRootAsGameEventStopToR_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_vec3Pos = new Vector3(data.Pos.X, data.Pos.Y, data.Pos.Z);

        return true;
    }
}
