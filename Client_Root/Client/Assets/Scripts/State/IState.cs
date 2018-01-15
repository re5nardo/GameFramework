using System.Collections.Generic;

public abstract class IState : ITickUpdatable
{
    protected int m_nMasterDataID = -1;

    protected float m_fLength;
    protected List<CoreState> m_vecCoreState;

    protected IEntity m_Entity;

    public abstract void Initialize(IEntity entity, int nMasterDataID, long lStartTime);

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    public bool HasCoreState(CoreState coreState)
    {
        return m_vecCoreState.Exists(x => x == coreState);
    }

    public virtual void OnCollision(IEntity other, long lTime)
    {
    }
};