using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMagicObject : ITickUpdatable
{
    protected int m_nCasterID = -1;
    protected int m_nMagicID = -1;
    protected int m_nID = -1;
    protected int m_nMasterDataID = -1;
    protected float m_fTickInterval = 0;
    protected int m_nStartTick = -1;
    protected int m_nEndTick = -1;
    protected float m_fLength;

    protected GameObject m_goModel = null;
    protected Transform m_trModel = null;
    protected Rigidbody m_ModelRigidbody = null;

    public abstract void Initialize(int nCasterID, int nMagicID, int nID, int nMasterDataID, float fTickInterval);

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
    }

    public Rigidbody GetRigidbody()
    {
        return m_ModelRigidbody;
    }

    public GameObject GetModel()
    {
        return m_goModel;
    }
}