using FlatBuffers;
using System.Collections.Generic;

public class WorldInfoToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.WorldInfoToC_ID;

    public int m_nTick;
    public float m_fStartTime;      //  Exclude
    public float m_fEndTime;        //  Include
    public List<IGameEvent> m_listGameEvent = new List<IGameEvent>();

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
//        var OffsetEntityState = new Offset<EntityState>[m_listEntityState.Count];

//        for (int i = 0; i < m_listEntityState.Count; ++i)
//        {
//            EntityState entityState = m_listEntityState[i];
//
//            var pos = FBSData.Vector3.CreateVector3(builder, entityState.Position.X, entityState.Position.Y, entityState.Position.Z);
//            var rot = FBSData.Vector3.CreateVector3(builder, entityState.Rotation.X, entityState.Rotation.Y, entityState.Rotation.Z);
//
//            int[] keys = new int[entityState.BehaviorsMapKeyLength];
//            float[] values = new float[entityState.BehaviorsMapValueLength];
//
//            for (int j = 0; j < entityState.BehaviorsMapKeyLength; ++j)
//            {
//                keys[j] = entityState.GetBehaviorsMapKey(j);
//                values[j] = entityState.GetBehaviorsMapValue(j);
//            }
//
//            var behaviorsMapKey = EntityState.CreateBehaviorsMapKeyVector(builder, keys);
//            var behaviorsMapValue = EntityState.CreateBehaviorsMapValueVector(builder, values);
//
//            EntityState.StartEntityState(builder);
//            EntityState.AddPlayerIndex(builder, entityState.PlayerIndex);
//            EntityState.AddPosition(builder, pos);
//            EntityState.AddRotation(builder, rot);
//            EntityState.AddBehaviorsMapKey(builder, behaviorsMapKey);
//            EntityState.AddBehaviorsMapValue(builder, behaviorsMapValue);
//
//            OffsetEntityState[i] = EntityState.EndEntityState(builder);
//        }
//
//        var entityStates = WorldSnapShotToC_Data.CreateEntityStatesVector(builder, OffsetEntityState);
//
//        WorldSnapShotToC_Data.StartWorldSnapShotToC_Data(builder);
//        WorldSnapShotToC_Data.AddTick(builder, m_nTick);
//        WorldSnapShotToC_Data.AddTime(builder, m_fTime);
//        WorldSnapShotToC_Data.AddEntityStates(builder, entityStates);
        var data = WorldSnapShotToC_Data.EndWorldSnapShotToC_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.WorldInfoToC.GetRootAsWorldInfoToC(buf);

        m_nTick = data.Tick;
        m_fStartTime = data.StartTime;
        m_fEndTime = data.EndTime;
        for (int i = 0; i < data.GameEventsLength; ++i)
        {
            FBS.GameEventData gameEventData = data.GetGameEvents(i);

            List<byte> listByte = new List<byte>();
            for (int j = 0; j < gameEventData.DataLength; ++j)
            {
                listByte.Add((byte)gameEventData.GetData(j));
            }
                
            if (gameEventData.Type == FBS.GameEventType.BehaviorStart)
            {
                GameEvent.BehaviorStart gameEvent = new GameEvent.BehaviorStart();
                gameEvent.m_fEventTime = gameEventData.EventTime;
                gameEvent.Deserialize(listByte.ToArray());

                m_listGameEvent.Add(gameEvent);
            }
            else if (gameEventData.Type == FBS.GameEventType.BehaviorEnd)
            {
                GameEvent.BehaviorEnd gameEvent = new GameEvent.BehaviorEnd();
                gameEvent.m_fEventTime = gameEventData.EventTime;
                gameEvent.Deserialize(listByte.ToArray());

                m_listGameEvent.Add(gameEvent);
            }
            else if (gameEventData.Type == FBS.GameEventType.Position)
            {
                GameEvent.Position gameEvent = new GameEvent.Position();
                gameEvent.m_fEventTime = gameEventData.EventTime;
                gameEvent.Deserialize(listByte.ToArray());

                m_listGameEvent.Add(gameEvent);
            }
            else if (gameEventData.Type == FBS.GameEventType.Rotation)
            {
                GameEvent.Rotation gameEvent = new GameEvent.Rotation();
                gameEvent.m_fEventTime = gameEventData.EventTime;
                gameEvent.Deserialize(listByte.ToArray());

                m_listGameEvent.Add(gameEvent);
            }
        }

        return true;
    }
}
