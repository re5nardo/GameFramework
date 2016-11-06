using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MisterBae : ICharacter
{
#region ICharacter
    protected override void CreateUI()
    {
        m_CharacterUI = (Instantiate(Resources.Load("CharacterUI/MisterBaeUI")) as GameObject).GetComponent<MisterBaeUI>();
    }

    public override void Initialize(params object[] arrParam)
    {
        m_DefaultStat = m_CurrentStat = (Stat)arrParam[0];
    }

    public override void Idle()
    {
        StopAllBehaviors();

        StartBehavior(new IdleBehavior(this, "WAIT01"));
    }

    public override void Stop()
    {
        StopAllBehaviors();

        StartBehavior(new StopBehavior(this));
    }

    public override void Move(LinkedList<Node> listPath)
    {
        StopAllBehaviors();

        StartBehavior(new MoveBehavior(this, listPath, "RUN00_F", m_LastBehavior is MoveBehavior));
    }

    public override void Skiil(object data)
    {
        
    }

    public override void Emotion()
    {
        
    }
#endregion
}
