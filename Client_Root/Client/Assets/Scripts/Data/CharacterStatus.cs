using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStatus
{
    public CharacterStatus(FBS.Data.CharacterStatus status)
    {
        m_nMaximumHP = status.MaximumHP;
        m_nHP = status.HP;
        m_nMaximumMP = status.MaximumMP;
        m_nMP = status.MP;
        m_fMaximumSpeed = status.MaximumSpeed;
        m_fSpeed = status.Speed;
        m_fMPChargeRate = status.MPChargeRate;
        m_fMovePoint = status.MovePoint;
    }
    public CharacterStatus(int nMaximumHP, int nHP, int nMaximumMP, int nMP, float fMaximumSpeed, float fSpeed, float fMPChargeRate, float fMovePoint)
    {
        m_nMaximumHP = nMaximumHP;
        m_nHP = nHP;
        m_nMaximumMP = nMaximumMP;
        m_nMP = nMP;
        m_fMaximumSpeed = fMaximumSpeed;
        m_fSpeed = fSpeed;
        m_fMPChargeRate = fMPChargeRate;
        m_fMovePoint = fMovePoint;
    }

    public int m_nMaximumHP;
    public int m_nHP;
    public int m_nMaximumMP;
    public int m_nMP;
    public float m_fMaximumSpeed;
    public float m_fSpeed;
    public float m_fMPChargeRate;
    public float m_fMovePoint;
}

public enum CoreState
{
    CoreState_Invincible = 0,
    CoreState_ChallengerDisturbing,
};