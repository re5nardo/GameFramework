using System.Collections.Generic;

public abstract class IBehavior : ITickUpdatable, IPooledObject
{
    protected DestroyType m_DestroyType_ = DestroyType.Normal;
    public DestroyType m_DestroyType { get { return m_DestroyType_; } set { m_DestroyType_ = value; } }

    protected System.DateTime m_StartTime_ = System.DateTime.Now;
    public System.DateTime m_StartTime { get { return m_StartTime_; } set { m_StartTime_ = value; } }

    protected bool m_bInUse_ = false;
    public bool m_bInUse { get { return m_bInUse_; } set { m_bInUse_ = value; } }

    protected int m_nMasterDataID = -1;
    protected IEntity m_Entity;

    protected float m_fLength;
    protected string  m_strStringParams;
    protected List<MasterData.Behavior.Action> m_vecAction;

    protected bool m_bActivated = false;

    public virtual void OnUsed() {}
    public virtual void OnReturned() {}
    public abstract void Initialize(IEntity entity, int nMasterDataID);

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    public override void StartTick(float fTickInterval, int nStartTick, params object[] param)
    {
        base.StartTick(fTickInterval, nStartTick, param);

        if (m_bActivated)
            return;

        m_bActivated = true;

        MasterData.Behavior masterData = null;
        MasterDataManager.Instance.GetData<MasterData.Behavior>(m_nMasterDataID, ref masterData);

        if (masterData.m_strAnimationName != "")
        {
            m_Entity.Play(masterData.m_strAnimationName, nStartTick, masterData.m_fLength < 0);
        }

        if (masterData.m_strName != "Idle")
        {
            IBehavior idle = m_Entity.GetBehavior(BehaviorID.IDLE);

            if (idle != null && idle.IsActivated())
            {
                idle.Stop();
            }
        }
    }

    public void Stop()
    {
        if (!m_bActivated)
            return;

        m_bActivated = false;

        MasterData.Behavior masterData = null;
        MasterDataManager.Instance.GetData<MasterData.Behavior>(m_nMasterDataID, ref masterData);

        if (masterData.m_strAnimationName != "")
        {
            m_Entity.Stop(masterData.m_strAnimationName);
        }
    }

    public bool IsActivated()
    {
        return m_bActivated;
    }

    public void OnCollision(IEntity pOther, long lTime)
    {
    }
}