using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IBehavior
{
    public delegate void BehaviorDelegate(IBehavior behavior);

    protected BehaviorBasedObject   m_Performer = null;
    private Coroutine               m_BodyCoroutine = null;
    protected Coroutine             m_SubBehavior = null;

    public IBehavior(BehaviorBasedObject performer)
    {
        m_Performer = performer;
    }

    public Coroutine Start()
    {
        return m_Performer.StartCoroutine(Do());
    }

    private IEnumerator Do()
    {
        m_BodyCoroutine = m_Performer.StartCoroutine(Body());
        yield return m_BodyCoroutine;

        m_Performer.OnBehaviorEnd(this);
    }

    protected abstract IEnumerator Body();

    public void Stop()
    {
        m_Performer.StopCoroutine(m_BodyCoroutine);
        if(m_SubBehavior != null)
        {
            m_Performer.StopCoroutine(m_SubBehavior);
        }

        OnStop();

        m_Performer.OnBehaviorEnd(this);
    }

    protected virtual void OnStop()
    {
    }
}
    

//public class CombiBehavior : IBehavior
//{
//    protected override IEnumerator Body()
//    {
//        //m_SubCoroutine = new TestBehavior().Start();
//        yield return m_SubBehavior;
//
//        //m_SubCoroutine = new TestBehavior().Start();
//        yield return m_SubBehavior;
//    }
//}