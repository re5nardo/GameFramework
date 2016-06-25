using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IBehavior
{
    public delegate void BehaviorDelegate(IBehavior behavior);

    protected ICharacter        m_Character = null;
    private BehaviorDelegate    m_OnBehaviorEnd = null;
    protected Coroutine         m_BodyCoroutine = null;
    protected Coroutine         m_SubCoroutine = null;

    public IBehavior(ICharacter Character, BehaviorDelegate OnBehaviorEnd)
    {
        m_Character = Character;
        m_OnBehaviorEnd = OnBehaviorEnd;
    }

    public Coroutine Start()
    {
        return m_Character.StartCoroutine(Do());
    }

    private IEnumerator Do()
    {
        m_BodyCoroutine = m_Character.StartCoroutine(Body());
        yield return m_BodyCoroutine;

        if(m_OnBehaviorEnd != null)
        {
            m_OnBehaviorEnd(this);
        }
    }

    protected abstract IEnumerator Body();

    public void Stop()
    {
        m_Character.StopCoroutine(m_BodyCoroutine);
        if(m_SubCoroutine != null)
        {
            m_Character.StopCoroutine(m_SubCoroutine);
        }

        OnStop();

        if(m_OnBehaviorEnd != null)
        {
            m_OnBehaviorEnd(this);
        }
    }

    protected virtual void OnStop()
    {
        m_Character.m_CharacterUI.StopAnimation();
    }
}
    
//public class MoveBehaviorr : IBehavior
//{
//    Vector3 m_vec3Dest = Vector3.zero;
//
//    public MoveBehaviorr(Vector3 vec3Dest)
//    {
//        m_vec3Dest = vec3Dest;
//    }
//
//    protected override IEnumerator Body()
//    {
//        Debug.Log("[1] frameCount : " + Time.frameCount);
//        yield return null;
//
//        Debug.Log("[2] frameCount : " + Time.frameCount);
//        yield return new WaitForSeconds(3);
//
//        Debug.Log("[3] frameCount : " + Time.frameCount);
//    }
//}
//
//public class CombiBehavior : IBehavior
//{
//    protected override IEnumerator Body()
//    {
//        //m_SubCoroutine = new TestBehavior().Start();
//        yield return m_SubCoroutine;
//
//        //m_SubCoroutine = new TestBehavior().Start();
//        yield return m_SubCoroutine;
//    }
//}