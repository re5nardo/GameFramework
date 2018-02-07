using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonoTickUpdatable : MonoBehaviour
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
