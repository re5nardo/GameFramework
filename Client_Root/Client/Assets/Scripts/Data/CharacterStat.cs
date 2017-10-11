using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStat
{
    public CharacterStat(int nHP, int nMP, float fMPChargeRate, float fRunSpeed, float fDashSpeed, float fStrength)
    {
        m_nHP = nHP;
        m_nMP = nMP;
        m_fMPChargeRate = fMPChargeRate;
        m_fRunSpeed = fRunSpeed;
        m_fDashSpeed = fDashSpeed;
        m_fStrength = fStrength;
    }

    public int m_nHP;
    public int m_nMP;
    public float m_fMPChargeRate;
    public float m_fRunSpeed;
    public float m_fDashSpeed;
    public float m_fStrength;
}