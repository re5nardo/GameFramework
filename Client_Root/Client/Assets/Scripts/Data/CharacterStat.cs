using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStat
{
    public CharacterStat(int nHP, int nMP, float fMPChargeRate, float fMaximumSpeed, float fStrength)
    {
        m_nHP = nHP;
        m_nMP = nMP;
        m_fMPChargeRate = fMPChargeRate;
        m_fMaximumSpeed = fMaximumSpeed;
        m_fStrength = fStrength;
    }

    public int m_nHP;
    public int m_nMP;
    public float m_fMPChargeRate;
    public float m_fMaximumSpeed;
    public float m_fStrength;
}