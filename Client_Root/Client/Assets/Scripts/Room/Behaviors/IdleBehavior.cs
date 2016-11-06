using UnityEngine;
using System.Collections;

public class IdleBehavior : IBehavior
{
    private ICharacter      m_Character = null;
    private string          m_strIdleClipName = "";

    public IdleBehavior(ICharacter Character, string strIdleClipName) : base(Character)
    {
        m_Character = Character;
        m_strIdleClipName = strIdleClipName;
    }

    protected override IEnumerator Body()
    {
        float fClipLength = m_Character.m_CharacterUI.GetAnimationClipLegth(m_strIdleClipName);
        float fElapsedTime = 0f;

        while (true)
        {
            m_Character.m_CharacterUI.SampleAnimation(m_strIdleClipName, (fElapsedTime % fClipLength) / fClipLength);

            yield return null;

            fElapsedTime += Time.deltaTime;
        }
    }
}
