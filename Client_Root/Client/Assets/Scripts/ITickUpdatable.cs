using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITickUpdatable
{
    protected float m_fTickInterval = 0;
    protected int m_nStartTick = -1;
    private int m_nLastUpdateTick = -1;

    public virtual void Start(float fTickInterval, int nStartTick, params object[] param)
    {
        m_fTickInterval = fTickInterval;
        m_nStartTick = nStartTick;
    }

    public void Update(int nUpdateTick)
    {
        if (m_nLastUpdateTick == nUpdateTick)
            return;
        
        UpdateBody(nUpdateTick);

        m_nLastUpdateTick = nUpdateTick;
    }

    protected abstract void UpdateBody(int nUpdateTick);
}


//    protected abstract void UpdateBody(float fStartTime, float fEndTime, bool bIncludeStartTime = false);