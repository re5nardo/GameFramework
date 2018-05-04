using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonoTickUpdatable : MonoBehaviour
{
    private int m_nLastUpdateTick = -1;
	protected bool m_bPredictPlay = false;

    public void UpdateTick(int nUpdateTick)
    {
        if (m_nLastUpdateTick >= nUpdateTick)
            return;

		if(BaeGameRoom2.Instance.IsPredictMode())
		{
			if(m_bPredictPlay)
			{
        		UpdateBody(nUpdateTick);
			}
		}
		else
		{
			UpdateBody(nUpdateTick);

        	m_nLastUpdateTick = nUpdateTick;
    	}
    }

    protected abstract void UpdateBody(int nUpdateTick);
}