using UnityEngine;
using System.Collections;

public class StopBehavior : IBehavior
{
    private const float     STOP_DURATION = 2f;

    private ICharacter      m_Character = null;

    public StopBehavior(ICharacter Character) : base(Character)
    {
        m_Character = Character;
    }

    protected override IEnumerator Body()
    {
        m_Character.m_CharacterUI.StopAnimation();

        yield return new WaitForSeconds(STOP_DURATION);
    }
}