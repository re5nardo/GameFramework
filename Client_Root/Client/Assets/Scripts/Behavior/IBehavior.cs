
public abstract class IBehavior : IPooledObject
{
    protected DestroyType m_DestroyType_ = DestroyType.Normal;
    public DestroyType m_DestroyType
    {
        get
        {
            return m_DestroyType_;
        }
        set
        {
            m_DestroyType_ = value;
        }
    }

    protected Entity m_Entity;

    public abstract bool Update();

    public virtual void OnUsed()
    {
    }

    public virtual void OnReturned()
    {
    }
}