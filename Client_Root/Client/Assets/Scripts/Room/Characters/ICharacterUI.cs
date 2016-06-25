﻿using UnityEngine;
using System.Collections;

public class ICharacterUI : MonoBehaviour
{
    [SerializeField] private Animation m_animCharacterUI = null;

    private Transform m_trCharacterUI = null;

    public float PlayAnimation(string strClipName, float fFadeLength = 0.2f)
    {
        m_animCharacterUI.CrossFadeQueued(strClipName, fFadeLength, QueueMode.PlayNow, PlayMode.StopSameLayer);

        return m_animCharacterUI[strClipName].length;
    }

    public void StopAnimation()
    {
        m_animCharacterUI.Stop();
    }

    public float GetAnimationClipLegth(string strClipName)
    {
        return m_animCharacterUI[strClipName].length;
    }

    public void SampleAnimation(string strClipName, float fNormalizedTime)
    {
        m_animCharacterUI[strClipName].normalizedTime = fNormalizedTime;
        m_animCharacterUI[strClipName].enabled = true;
        m_animCharacterUI[strClipName].weight = 1f;
        m_animCharacterUI.Sample();
        m_animCharacterUI[strClipName].enabled = false;
    }

    //  local pos
    public void SetPosition(Vector3 vec3Pos)
    {
        m_trCharacterUI.localPosition = vec3Pos;
    }
}
