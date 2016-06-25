using UnityEngine;
using System.Collections;

public class StopBehavior : IBehavior
{
    private const float STOP_DURATION = 2f;

    public StopBehavior(ICharacter Character, BehaviorDelegate OnBehaviorEnd) : base(Character, OnBehaviorEnd)
    {
    }

    protected override IEnumerator Body()
    {
        m_Character.m_CharacterUI.StopAni();

        yield return new WaitForSeconds(STOP_DURATION);
    }
}