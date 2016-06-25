using UnityEngine;
using System.Collections;

public class MisterBae : ICharacter
{
    public MisterBae(Stat stat)
    {
        m_DefaultStat = m_CurrentStat = stat;
    }

#region ICharacter
    protected override void CreateUI()
    {
        
    }

    public override void Idle()
    {
        foreach(IBehavior behavior in m_listBehavior)
        {
            behavior.Stop();
        }
        m_listBehavior.Clear();

        IdleBehavior idleBehavior = new IdleBehavior(this, OnBehaviorEnd);

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

    public override void Move(Vector3 vec3Pos)
    {
        IBehavior oldMoveBehavior = m_listBehavior.Find(behavior => behavior is MoveBehavior);

        if (oldMoveBehavior != null)
        {
            oldMoveBehavior.Stop();
        }
        m_listBehavior.Remove(oldMoveBehavior);

        MoveBehavior moveBehavior = new MoveBehavior(this, OnBehaviorEnd, null);

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
