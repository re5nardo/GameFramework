using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledComponent : MonoBehaviour, IPooledObject
{
    public string m_strKey = "";

    protected DestroyType m_DestroyType_ = DestroyType.Normal;
    public DestroyType m_DestroyType
    {
        get
        {
            return m_DestroyType_;
        }
        set
        {
            m_DestroyType_ = value;
        }
    }

    public virtual void OnUsed()
    {
    }

    public virtual void OnReturned()
    {
    }
}
