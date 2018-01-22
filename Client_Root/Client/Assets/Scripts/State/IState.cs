using System.Collections.Generic;

public abstract class IState : ITickUpdatable
{
    protected int m_nMasterDataID = -1;

    protected float m_fLength;
    protected List<CoreState> m_listCoreState = new List<CoreState>();

    protected IEntity m_Entity;

    public abstract void Initialize(IEntity entity, int nMasterDataID);

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    public bool HasCoreState(CoreState coreState)
    {
        return m_listCoreState.Exists(x => x == coreState);
    }

    public virtual void OnCollision(IEntity other, int nTick)
    {
    }
};