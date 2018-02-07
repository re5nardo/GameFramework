using System.Collections.Generic;

public abstract class IBehavior : ITickUpdatable
{
    protected float m_fTickInterval = 0;
    protected int m_nStartTick = -1;

    protected int m_nMasterDataID = -1;
    protected IEntity m_Entity;

    protected float m_fLength;
    protected string  m_strStringParams;
    protected List<MasterData.Behavior.Action> m_listAction;

    protected bool m_bActivated = false;

    public virtual void OnUsed() {}
    public virtual void OnReturned() {}
    public abstract void Initialize(IEntity entity, int nMasterDataID, float fTickInterval);

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    public virtual void StartTick(int nStartTick, params object[] param)
    {
        m_nStartTick = nStartTick;

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