using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviorBasedObject : MonoBehaviour
{
    private List<IBehavior> m_listBehavior = new List<IBehavior>();
    protected IBehavior     m_LastBehavior = null;

    protected void StartBehavior(IBehavior behavior)
    {
        m_LastBehavior = behavior;

        m_listBehavior.Add(behavior);

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
        m_listBehavior.Remove(behavior);
    }

    protected bool IsBehaviorPlaying()
    {
        return m_listBehavior.Count != 0;
    }
}