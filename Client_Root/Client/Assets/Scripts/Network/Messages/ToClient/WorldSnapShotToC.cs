using FlatBuffers;
using System.Collections.Generic;

public class WorldSnapShotToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.WorldSnapShotToC_ID;

    public int m_nTick;
    public float m_fTime;
    public List<FBS.EntityState> m_listEntityState = new List<FBS.EntityState>();

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
        var OffsetEntityState = new Offset<FBS.EntityState>[m_listEntityState.Count];

        for (int i = 0; i < m_listEntityState.Count; ++i)
        {
            FBS.EntityState entityState = m_listEntityState[i];

            var pos = FBS.Data.Vector3.CreateVector3(m_Builder, entityState.Position.X, entityState.Position.Y, entityState.Position.Z);
            var rot = FBS.Data.Vector3.CreateVector3(m_Builder, entityState.Rotation.X, entityState.Rotation.Y, entityState.Rotation.Z);

            int[] keys = new int[entityState.BehaviorsMapKeyLength];
            float[] values = new float[entityState.BehaviorsMapValueLength];

            for (int j = 0; j < entityState.BehaviorsMapKeyLength; ++j)
            {
                keys[j] = entityState.GetBehaviorsMapKey(j);
                values[j] = entityState.GetBehaviorsMapValue(j);
            }

            var behaviorsMapKey = FBS.EntityState.CreateBehaviorsMapKeyVector(m_Builder, keys);
            var behaviorsMapValue = FBS.EntityState.CreateBehaviorsMapValueVector(m_Builder, values);

            FBS.EntityState.StartEntityState(m_Builder);
            FBS.EntityState.AddPlayerIndex(m_Builder, entityState.PlayerIndex);
            FBS.EntityState.AddPosition(m_Builder, pos);
            FBS.EntityState.AddRotation(m_Builder, rot);
            FBS.EntityState.AddBehaviorsMapKey(m_Builder, behaviorsMapKey);
            FBS.EntityState.AddBehaviorsMapValue(m_Builder, behaviorsMapValue);

            OffsetEntityState[i] = FBS.EntityState.EndEntityState(m_Builder);
        }

        var entityStates = FBS.WorldSnapShotToC.CreateEntityStatesVector(m_Builder, OffsetEntityState);

        FBS.WorldSnapShotToC.StartWorldSnapShotToC(m_Builder);
        FBS.WorldSnapShotToC.AddTick(m_Builder, m_nTick);
        FBS.WorldSnapShotToC.AddTime(m_Builder, m_fTime);
        FBS.WorldSnapShotToC.AddEntityStates(m_Builder, entityStates);
        var data = FBS.WorldSnapShotToC.EndWorldSnapShotToC(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.WorldSnapShotToC.GetRootAsWorldSnapShotToC(buf);

        m_nTick = data.Tick;
        m_fTime = data.Time;
        for (int i = 0; i < data.EntityStatesLength; ++i)
        {
            m_listEntityState.Add(data.GetEntityStates(i));
        }

        return true;
    }

    public override void OnReturned()
    {
        m_listEntityState.Clear();
    }
}
