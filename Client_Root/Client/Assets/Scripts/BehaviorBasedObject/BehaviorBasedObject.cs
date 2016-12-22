using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviorBasedObject : MonoBehaviour
{
    protected List<IBehavior>   m_listBehavior = new List<IBehavior>();
    protected IBehavior         m_LastBehavior = null;

    private Dictionary<IBehavior, System.Action> m_dicCallBack = new Dictionary<IBehavior, System.Action>();

    protected void StartBehavior(IBehavior behavior, System.Action callback = null)
    {
        m_LastBehavior = behavior;

        m_listBehavior.Add(behavior);

        m_dicCallBack.Add(behavior, callback);

        behavior.Start();
    }

    protected void StopBehavior(IBehavior behavior)
    {
        if(m_listBehavior.Find(a => a == behavior) == null)
        {
            Debug.LogWarning("Can't stop behavior. behavior is invalid!");
            return;
        }

        behavior.Stop();
        m_listBehavior.Remove(behavior);
    }

    protected void StopAllBehaviors()
    {
        for(int i = m_listBehavior.Count - 1; i >= 0; --i)
        {
            StopBehavior(m_listBehavior[i]);
        }
    }

    public void OnBehaviorEnd(IBehavior behavior)
    {
        //  Ignore sub behavior
        if (!m_dicCallBack.ContainsKey(behavior))
            return;

        if (m_dicCallBack[behavior] != null)
            m_dicCallBack[behavior]();

        m_dicCallBack.Remove(behavior);

        m_listBehavior.Remove(behavior);
    }

    protected int GetCountOfPlayingBehavior()
    {
        return m_listBehavior.Count;
    }
}