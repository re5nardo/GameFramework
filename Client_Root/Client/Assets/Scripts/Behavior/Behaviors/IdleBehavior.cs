using UnityEngine;
using System.Collections;

public class IdleBehavior : IBehavior
{
    private ICharacter      m_Character = null;
    private string          m_strIdleClipName = "";
    private bool            m_bContinue = false;

    public IdleBehavior(ICharacter Character, string strIdleClipName, bool bContinue) : base(Character)
    {
        m_Character = Character;
        m_strIdleClipName = strIdleClipName;
        m_bContinue = bContinue;
    }

    protected override IEnumerator Body()
    {
        float fClipLength = m_Character.m_CharacterUI.GetAnimationClipLegth(m_strIdleClipName);
        float fElapsedTime = 0f;
        float fContinueTime = m_bContinue ? m_Character.m_CharacterUI.GetAnimationStateTime(m_strIdleClipName) : 0f;

        while (true)
        {
            m_Character.m_CharacterUI.SampleAnimation(m_strIdleClipName, ((fElapsedTime + fContinueTime) % fClipLength) / fClipLength);

            yield return null;

            fElapsedTime += Time.deltaTime;
        }
    }
}
