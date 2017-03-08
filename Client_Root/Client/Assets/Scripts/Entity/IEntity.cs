using UnityEngine;
using System.Collections.Generic;

public abstract class IEntity : MonoBehaviour
{
    protected IEntityUI             m_EntityUI = null;
    protected Vector3               m_vec3Position = Vector3.zero;
    protected Stat                  m_DefaultStat = default(Stat);
    protected Stat                  m_CurrentStat = default(Stat);

    protected abstract void CreateUI();
    public abstract void Initialize(params object[] arrParam);
    public abstract void SampleBehaviors(Dictionary<string, KeyValuePair<float, float>> dicBehaviors, float fInterpolationValue, float fTickInterval, float fEmptyValue);

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

        m_EntityUI.SetPosition(vec3Pos);
    }

    public Vector3 GetPosition()
    {
        return m_vec3Position;
    }

    public Transform GetUITransform()
    {
        return m_EntityUI.transform;
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();

        if (m_EntityUI != null)
        {
            Destroy(m_EntityUI.gameObject);
        }
    }
}
