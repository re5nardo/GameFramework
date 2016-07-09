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
        foreach(IBehavior behavior in m_listBehavior)
        {
            behavior.Stop();
        }
        m_listBehavior.Clear();

        IdleBehavior idleBehavior = new IdleBehavior(this, OnBehaviorEnd, "WAIT01");

        m_listBehavior.Add(idleBehavior);

        idleBehavior.Start();
    }

    public override void Stop()
    {
        foreach(IBehavior behavior in m_listBehavior)
        {
            behavior.Stop();
        }
        m_listBehavior.Clear();

        StopBehavior stopBehavior = new StopBehavior(this, OnBehaviorEnd);

        m_listBehavior.Add(stopBehavior);

        stopBehavior.Start();
    }

    public override void Move(LinkedList<Node> listPath)
    {
        List<IBehavior> listBehavior = m_listBehavior.FindAll(behavior => behavior is MoveBehavior || behavior is IdleBehavior );

        foreach(IBehavior behavior in listBehavior)
        {
            behavior.Stop();
            m_listBehavior.Remove(behavior);
        }

        MoveBehavior moveBehavior = new MoveBehavior(this, OnBehaviorEnd, listPath, "RUN00_F");

        m_listBehavior.Add(moveBehavior);

        moveBehavior.Start();
    }

    public override void Skiil(object data)
    {
        
    }

    public override void Emotion()
    {
        
    }
#endregion

    private void OnBehaviorEnd(IBehavior behavior)
    {
        m_listBehavior.Remove(behavior);
    }
}
