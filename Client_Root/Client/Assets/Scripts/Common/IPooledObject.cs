
public enum DestroyType
{
    Normal,
    SceneChange,
    NeverDestroyed,
}

public interface IPooledObject
{
    DestroyType m_DestroyType { get; set; }
    System.DateTime m_StartTime { get; set; }
    bool m_bInUse { get; set; }

    void OnUsed();
    void OnReturned();
}