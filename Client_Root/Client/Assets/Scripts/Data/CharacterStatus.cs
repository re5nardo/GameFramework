using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStatus
{
    public CharacterStatus(int nMaximumHP, int nHP, int nMaximumMP, int nMP, float fMaximumSpeed, float fSpeed, float fMPChargeRate, int nMaximumJumpCount, int nJumpCount, float fJumpRegenerationTime, float fMovePoint)
    {
        m_nMaximumHP = nMaximumHP;
        m_nHP = nHP;
        m_nMaximumMP = nMaximumMP;
        m_nMP = nMP;
        m_fMaximumSpeed = fMaximumSpeed;
        m_fSpeed = fSpeed;
        m_fMPChargeRate = fMPChargeRate;
        m_nMaximumJumpCount = nMaximumJumpCount;
        m_nJumpCount = nJumpCount;
        m_fJumpRegenerationTime = fJumpRegenerationTime;
        m_fMovePoint = fMovePoint;
    }

    public int m_nMaximumHP;
    public int m_nHP;
    public int m_nMaximumMP;
    public int m_nMP;
    public float m_fMaximumSpeed;
    public float m_fSpeed;
    public float m_fMPChargeRate;
    public int m_nMaximumJumpCount;
    public int m_nJumpCount;
    public float m_fJumpRegenerationTime;
    public float m_fMovePoint;
}