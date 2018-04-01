using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMagic : ITickUpdatable
{
    protected int m_nCasterID = -1;
    protected int m_nMasterDataID = -1;
    protected float m_fTickInterval = 0;

    public abstract void Initialize(int nCasterID, int nMasterDataID, float fTickInterval);
}