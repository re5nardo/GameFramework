using FlatBuffers;

public abstract class IGameEvent : IDeserializable
{
    protected FlatBufferBuilder m_Builder = new FlatBufferBuilder(1024);
    public float m_fEventTime;

    public abstract FBS.GameEventType GetEventType();
    public abstract bool Deserialize(byte[] bytes);

    public override string ToString()
    {
        return string.Format("EventType : {0}, EventTime : {1}", GetEventType().ToString(), m_fEventTime);
    }
}