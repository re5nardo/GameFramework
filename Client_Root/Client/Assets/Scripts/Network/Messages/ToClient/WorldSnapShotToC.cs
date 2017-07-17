using FlatBuffers;
using System.Collections.Generic;

public class WorldSnapShotToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.WorldSnapShotToC_ID;

    public int m_nTick;
    public float m_fTime;
    public List<EntityState> m_listEntityState = new List<EntityState>();

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
        var OffsetEntityState = new Offset<EntityState>[m_listEntityState.Count];

        for (int i = 0; i < m_listEntityState.Count; ++i)
        {
            EntityState entityState = m_listEntityState[i];

            var pos = FBSData.Vector3.CreateVector3(m_Builder, entityState.Position.X, entityState.Position.Y, entityState.Position.Z);
            var rot = FBSData.Vector3.CreateVector3(m_Builder, entityState.Rotation.X, entityState.Rotation.Y, entityState.Rotation.Z);

            int[] keys = new int[entityState.BehaviorsMapKeyLength];
            float[] values = new float[entityState.BehaviorsMapValueLength];

            for (int j = 0; j < entityState.BehaviorsMapKeyLength; ++j)
            {
                keys[j] = entityState.GetBehaviorsMapKey(j);
                values[j] = entityState.GetBehaviorsMapValue(j);
            }

            var behaviorsMapKey = EntityState.CreateBehaviorsMapKeyVector(m_Builder, keys);
            var behaviorsMapValue = EntityState.CreateBehaviorsMapValueVector(m_Builder, values);

            EntityState.StartEntityState(m_Builder);
            EntityState.AddPlayerIndex(m_Builder, entityState.PlayerIndex);
            EntityState.AddPosition(m_Builder, pos);
            EntityState.AddRotation(m_Builder, rot);
            EntityState.AddBehaviorsMapKey(m_Builder, behaviorsMapKey);
            EntityState.AddBehaviorsMapValue(m_Builder, behaviorsMapValue);

            OffsetEntityState[i] = EntityState.EndEntityState(m_Builder);
        }

        var entityStates = WorldSnapShotToC_Data.CreateEntityStatesVector(m_Builder, OffsetEntityState);

        WorldSnapShotToC_Data.StartWorldSnapShotToC_Data(m_Builder);
        WorldSnapShotToC_Data.AddTick(m_Builder, m_nTick);
        WorldSnapShotToC_Data.AddTime(m_Builder, m_fTime);
        WorldSnapShotToC_Data.AddEntityStates(m_Builder, entityStates);
        var data = WorldSnapShotToC_Data.EndWorldSnapShotToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
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
