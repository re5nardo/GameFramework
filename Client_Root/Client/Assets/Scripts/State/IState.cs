using System.Collections.Generic;

public abstract class IState : ITickUpdatable, IPooledObject
{
    protected DestroyType m_DestroyType_ = DestroyType.Normal;
    public DestroyType m_DestroyType { get { return m_DestroyType_; } set { m_DestroyType_ = value; } }

    protected System.DateTime m_StartTime_ = System.DateTime.Now;
    public System.DateTime m_StartTime { get { return m_StartTime_; } set { m_StartTime_ = value; } }

    protected bool m_bInUse_ = false;
    public bool m_bInUse { get { return m_bInUse_; } set { m_bInUse_ = value; } }

    protected int m_nMasterDataID = -1;

    protected float m_fLength;
	protected string m_strFxName;
    protected List<CoreState> m_listCoreState = new List<CoreState>();

    protected IEntity m_Entity;

    protected float m_fTickInterval = 0;
    protected int m_nStartTick = -1;
    protected int m_nEndTick = -1;

    public virtual void StartTick(int nStartTick, params object[] param)
    {
        m_nStartTick = nStartTick;
        if (m_fLength == -1)
        {
            m_nEndTick = -1;
        }
        else
        {
            m_nEndTick = nStartTick + (int)(m_fLength / m_fTickInterval) - 1;
        }

        MasterData.State masterData = null;
        MasterDataManager.Instance.GetData<MasterData.State>(m_nMasterDataID, ref masterData);

        if (masterData.m_strAnimationName != "")
        {
            m_Entity.Play(masterData.m_strAnimationName, nStartTick, masterData.m_fLength < 0);
        }
    }

    public abstract void Initialize(IEntity entity, int nMasterDataID, float fTickInterval);

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

    public virtual void OnUsed()
    {
    }

    public virtual void OnReturned()
    {
    }
};