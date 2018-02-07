
public abstract class ITickUpdatable
{
    private int m_nLastUpdateTick = -1;

    public void UpdateTick(int nUpdateTick)
    {
        if (m_nLastUpdateTick == nUpdateTick)
            return;
        
        UpdateBody(nUpdateTick);

        m_nLastUpdateTick = nUpdateTick;
    }

    protected abstract void UpdateBody(int nUpdateTick);
}


//    protected abstract void UpdateBody(float fStartTime, float fEndTime, bool bIncludeStartTime = false);