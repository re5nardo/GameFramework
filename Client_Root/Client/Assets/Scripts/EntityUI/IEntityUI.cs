﻿using UnityEngine;
using System.Collections;

public class IEntityUI : MonoBehaviour
{
    [SerializeField] private Animation m_animEntityUI = null;

    private Transform m_trEntityUI = null;

    protected virtual void Awake()
    {
        m_trEntityUI = transform;
    }

    public float GetAnimationClipLegth(string strClipName)
    {
        return m_animEntityUI[strClipName].length;
    }

    public void SampleAnimation(string strClipName, float fNormalizedTime, float fWeight = 1f)
    {
        if (!IsClipNameValid(strClipName))
        {
            Debug.LogError("strClipName is invalid!, strClipName : " + strClipName);
            return;
        }

        m_animEntityUI[strClipName].normalizedTime = fNormalizedTime;
        m_animEntityUI[strClipName].enabled = true;
        m_animEntityUI[strClipName].weight = fWeight;
        m_animEntityUI.Sample();
        m_animEntityUI[strClipName].enabled = false;
    }

    public float GetAnimationStateTime(string strClipName)
    {
        if (!IsClipNameValid(strClipName))
        {
            Debug.LogError("strClipName is invalid!, strClipName : " + strClipName);
            return 0f;
        }

        return m_animEntityUI[strClipName].time;
    }

    private bool IsClipNameValid(string strClipName)
    {
        return m_animEntityUI.GetClip(strClipName) != null;
    }

    //  Local pos
    public void SetPosition(Vector3 vec3Pos)
    {
        m_trEntityUI.localPosition = vec3Pos;
    }
}