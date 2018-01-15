﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledComponent : MonoBehaviour, IPooledObject
{
    [HideInInspector] public string m_strKey = "";

    protected DestroyType m_DestroyType_ = DestroyType.Normal;
    public DestroyType m_DestroyType { get { return m_DestroyType_; } set { m_DestroyType_ = value; } }

    protected System.DateTime m_StartTime_ = System.DateTime.Now;
    public System.DateTime m_StartTime { get { return m_StartTime_; } set { m_StartTime_ = value; } }

    protected bool m_bInUse_ = false;
    public bool m_bInUse { get { return m_bInUse_; } set { m_bInUse_ = value; } }

    public virtual void OnUsed()
    {
    }

    public virtual void OnReturned()
    {
    }
}
