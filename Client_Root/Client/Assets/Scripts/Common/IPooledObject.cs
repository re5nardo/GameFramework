
public enum DestroyType
{
    Normal,
    SceneChange,
    NeverDestroyed,
}

public interface IPooledObject
{
    DestroyType m_DestroyType { get; set; }

    void OnUsed();
    void OnReturned();
}