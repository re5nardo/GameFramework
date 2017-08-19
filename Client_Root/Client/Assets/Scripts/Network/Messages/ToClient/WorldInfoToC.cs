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
        return null;
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

            IGameEvent gameEvent = null;
            if (gameEventData.Type == FBS.GameEventType.BehaviorStart)
            {
                gameEvent = new GameEvent.BehaviorStart();
            }
            else if (gameEventData.Type == FBS.GameEventType.BehaviorEnd)
            {
                gameEvent = new GameEvent.BehaviorEnd();
            }
            else if (gameEventData.Type == FBS.GameEventType.Position)
            {
                gameEvent = new GameEvent.Position();
            }
            else if (gameEventData.Type == FBS.GameEventType.Rotation)
            {
                gameEvent = new GameEvent.Rotation();
            }
            else if (gameEventData.Type == FBS.GameEventType.EntityCreate)
            {
                gameEvent = new GameEvent.EntityCreate();
            }
            else if (gameEventData.Type == FBS.GameEventType.EntityDestroy)
            {
                gameEvent = new GameEvent.EntityDestroy();
            }

            gameEvent.m_fEventTime = gameEventData.EventTime;
            gameEvent.Deserialize(listByte.ToArray());

            m_listGameEvent.Add(gameEvent);
        }

        return true;
    }
}
