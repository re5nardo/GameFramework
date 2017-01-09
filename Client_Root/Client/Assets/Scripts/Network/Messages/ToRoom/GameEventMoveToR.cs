using UnityEngine;
using FlatBuffers;

public class GameEventMoveToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameEventMoveToR_ID;

    public int m_nPlayerIndex;
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

        GameEventMoveToR_Data.StartGameEventMoveToR_Data(builder);
        GameEventMoveToR_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameEventMoveToR_Data.AddDest(builder, dest);
        var data = GameEventMoveToR_Data.EndGameEventMoveToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameEventMoveToR_Data.GetRootAsGameEventMoveToR_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_vec3Dest = new Vector3(data.Dest.X, data.Dest.Y, data.Dest.Z);

        return true;
    }
}
