using UnityEngine;
using System.Collections.Generic;

public abstract class ICharacter : MonoBehaviour
{
    protected IBehavior m_BehaviorCurrent = null;



    public void Stop()
    {
        
    }

    public abstract void Move(Vector3 vec3Pos);
    public abstract void Skiil(object data);


    public void OnBehaviorEnd(IBehavior behavior)
    {
       
    }

    public void StopBehavior()
    {
        if (m_BehaviorCurrent != null)
        {
            m_BehaviorCurrent.Stop();
        }
    }
}
