using UnityEngine;
using System.Collections;

public class IdleBehavior : IBehavior
{
    private string                      m_strIdleClipName = "";

    public IdleBehavior(ICharacter Character, BehaviorDelegate OnBehaviorEnd, string strIdleClipName) : base(Character, OnBehaviorEnd)
    {
        m_strIdleClipName = strIdleClipName;
    }

    protected override IEnumerator Body()
    {
        float fClipLength = m_Character.m_CharacterUI.GetAnimationClipLegth(m_strIdleClipName);
        float fElapsedTime = 0f;

        while (true)
        {
            fElapsedTime = fElapsedTime % fClipLength;

            m_Character.m_CharacterUI.SampleAnimation(m_strIdleClipName, fElapsedTime / fClipLength);

            yield return null;

            fElapsedTime += Time.deltaTime;
        }
    }
}
