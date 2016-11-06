using UnityEngine;
using System.Collections.Generic;

public abstract class ICharacter : BehaviorBasedObject
{
    public ICharacterUI             m_CharacterUI = null;
    protected Vector3               m_vec3Position = Vector3.zero;
    protected Stat                  m_DefaultStat = default(Stat);
    protected Stat                  m_CurrentStat = default(Stat);

    public abstract void Idle();
    public abstract void Stop();
    public abstract void Move(LinkedList<Node> listPath);
    public abstract void Skiil(object data);
    public abstract void Emotion();

    protected abstract void CreateUI();
    public abstract void Initialize(params object[] arrParam);

    //  must be called!!
    protected virtual void Awake()
    {
        CreateUI();
    }

    public virtual float GetSpeed()
    {
        return m_CurrentStat.fSpeed;
    }

    public void SetPosition(Vector3 vec3Pos)
    {
        m_vec3Position = vec3Pos;

        m_CharacterUI.SetPosition(vec3Pos);
    }

    public Vector3 GetPosition()
    {
        return m_vec3Position;
    }

    protected virtual void LateUpdate()
    {
        if (!IsBehaviorPlaying())
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
