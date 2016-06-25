using System.Collections;

public class IdleBehavior : IBehavior
{
    public IdleBehavior(ICharacter Character, BehaviorDelegate OnBehaviorEnd) : base(Character, OnBehaviorEnd)
    {
    }

    protected override IEnumerator Body()
    {
        m_Character.m_CharacterUI.PlayAni("idle");
       
        while (true)
        {
            yield return null;
        }
    }
}
