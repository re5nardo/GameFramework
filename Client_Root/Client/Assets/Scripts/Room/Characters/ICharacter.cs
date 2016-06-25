using UnityEngine;
using System.Collections.Generic;

public abstract class ICharacter : MonoBehaviour    //  Inherit MonoBehaviour for coroutine..
{
    public ICharacterUI             m_CharacterUI = null;
    protected List<IBehavior>       m_listBehavior = new List<IBehavior>(8);

    protected Vector3               m_vec3Position = Vector3.zero;
    protected Stat                  m_DefaultStat = default(Stat);
    protected Stat                  m_CurrentStat = default(Stat);

    public abstract void Idle();
    public abstract void Stop();
    public abstract void Move(Vector3 vec3Pos);
    public abstract void Skiil(object data);
    public abstract void Emotion();

    protected abstract void CreateUI();
    public abstract void Initialize(params object[] arrParam);

    protected virtual void Awake()
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

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();

        if (m_CharacterUI != null)
        {
            Destroy(m_CharacterUI.gameObject);
        }
    }
}
