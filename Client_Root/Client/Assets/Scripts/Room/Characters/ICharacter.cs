using UnityEngine;
using System.Collections.Generic;

public abstract class ICharacter : MonoBehaviour    //  Inherit MonoBehaviour for coroutine..
{
    public ICharacterUI             m_CharacterUI = null;
    protected List<IBehavior>       m_listBehavior = new List<IBehavior>(8);

    protected Vector3               m_vec3Position = Vector3.zero;
    protected Stat                  m_DefaultStat;
    protected Stat                  m_CurrentStat;

    public abstract void Idle();
    public abstract void Stop();
    public abstract void Move(Vector3 vec3Pos);
    public abstract void Skiil(object data);
    public abstract void Emotion();

    protected abstract void CreateUI();

    public ICharacter()
    {
        CreateUI();
    }

    public virtual float GetSpeed()
    {
        return m_CurrentStat.fSpeed;
    }

    protected virtual void LateUpdate()
    {
        if (m_listBehavior.Count == 0)
        {
            Idle();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
