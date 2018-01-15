using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickBasedAnimationPlayer : MonoBehaviour
{
    private class InternalAnimState
    {
        public string m_strAnimation;
        public int m_nStartTick;
        public bool m_bLoop;
    }

    [SerializeField] private Animation m_Animation = null;

    private Dictionary<string, InternalAnimState> m_dicPlayingList = new Dictionary<string, InternalAnimState>();

    private float m_fTickInterval = 1;

    public void SetTickInterval(float fTickInterval)
    {
        m_fTickInterval = fTickInterval;
    }

    public void Play(string strAnimation, int nStartTick, bool bLoop = false, float fWeight = 1)
    {
        if(!m_dicPlayingList.ContainsKey(strAnimation))
        {
            m_dicPlayingList[strAnimation] = new InternalAnimState();
        }

        m_dicPlayingList[strAnimation].m_strAnimation = strAnimation;
        m_dicPlayingList[strAnimation].m_nStartTick = nStartTick;
        m_dicPlayingList[strAnimation].m_bLoop = bLoop;

        m_Animation[strAnimation].weight = fWeight;
    }

    public void Stop(string strAnimation)
    {
        m_dicPlayingList.Remove(strAnimation);
    }

    public void Draw(int nTick)
    {
        foreach(InternalAnimState state in m_dicPlayingList.Values)
        {
            AnimationState animationState = m_Animation[state.m_strAnimation];

            //            animationState.blendMode = AnimationBlendMode.Blend;
            animationState.enabled = true;

            if(animationState.wrapMode != WrapMode.Loop && state.m_bLoop)
            {
                animationState.time = ((nTick - state.m_nStartTick) * m_fTickInterval) % animationState.length;
            }
            else
            {
                animationState.time = (nTick - state.m_nStartTick) * m_fTickInterval;
            }
        }

        m_Animation.Sample();
        m_Animation.Stop();

        //    trim
        List<string> listToRemove = new List<string>();
        foreach(InternalAnimState state in m_dicPlayingList.Values)
        {
            AnimationState animationState = m_Animation[state.m_strAnimation];

            if(!state.m_bLoop)
            {
                if(animationState.length <= (nTick - state.m_nStartTick) * m_fTickInterval)
                {
                    listToRemove.Add(state.m_strAnimation);
                }
            }
        }

        foreach(string strAnimationName in listToRemove)
        {
            m_dicPlayingList.Remove(strAnimationName);
        }
    }
}