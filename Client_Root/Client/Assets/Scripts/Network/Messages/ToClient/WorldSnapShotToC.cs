using FlatBuffers;
using System.Collections.Generic;

public class WorldSnapShotToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.WorldSnapShotToC_ID;

    public int m_nTick;
    public float m_fTime;
    public List<EntityState> m_listEntityState = new List<EntityState>();

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

        var OffsetEntityState = new Offset<EntityState>[m_listEntityState.Count];

        for (int i = 0; i < m_listEntityState.Count; ++i)
        {
            EntityState entityState = m_listEntityState[i];

            var pos = FBSData.Vector3.CreateVector3(builder, entityState.Position.X, entityState.Position.Y, entityState.Position.Z);
            var rot = FBSData.Vector3.CreateVector3(builder, entityState.Rotation.X, entityState.Rotation.Y, entityState.Rotation.Z);

            int[] keys = new int[entityState.BehaviorsMapKeyLength];
            float[] values = new float[entityState.BehaviorsMapValueLength];

            for (int j = 0; j < entityState.BehaviorsMapKeyLength; ++j)
            {
                keys[j] = entityState.GetBehaviorsMapKey(j);
                values[j] = entityState.GetBehaviorsMapValue(j);
            }

            var behaviorsMapKey = EntityState.CreateBehaviorsMapKeyVector(builder, keys);
            var behaviorsMapValue = EntityState.CreateBehaviorsMapValueVector(builder, values);

            EntityState.StartEntityState(builder);
            EntityState.AddPlayerIndex(builder, entityState.PlayerIndex);
            EntityState.AddPosition(builder, pos);
            EntityState.AddBehaviorsMapKey(builder, behaviorsMapKey);
            EntityState.AddBehaviorsMapValue(builder, behaviorsMapValue);

            OffsetEntityState[i] = EntityState.EndEntityState(builder);
        }

        var entityStates = WorldSnapShotToC_Data.CreateEntityStatesVector(builder, OffsetEntityState);

        WorldSnapShotToC_Data.StartWorldSnapShotToC_Data(builder);
        WorldSnapShotToC_Data.AddTick(builder, m_nTick);
        WorldSnapShotToC_Data.AddTime(builder, m_fTime);
        WorldSnapShotToC_Data.AddEntityStates(builder, entityStates);
        var data = WorldSnapShotToC_Data.EndWorldSnapShotToC_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = WorldSnapShotToC_Data.GetRootAsWorldSnapShotToC_Data(buf);

        m_nTick = data.Tick;
        m_fTime = data.Time;
        for (int i = 0; i < data.EntityStatesLength; ++i)
        {
            m_listEntityState.Add(data.GetEntityStates(i));
        }

        return true;
    }
}
