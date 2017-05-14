using UnityEngine;
using System.Collections.Generic;

public abstract class IEntity : MonoBehaviour
{
    protected IEntityUI             m_EntityUI = null;
    protected Vector3               m_vec3Position = Vector3.zero;
    protected Vector3               m_vec3Rotation = Vector3.zero;

    protected abstract void CreateUI();
    public abstract void Initialize();
    public abstract void SampleBehaviors(Dictionary<string, KeyValuePair<float, float>> dicBehaviors, float fInterpolationValue, float fTickInterval, float fEmptyValue);

    //  must be called!!
    protected virtual void Awake()
    {
        CreateUI();
    }

    public void SetPosition(Vector3 vec3Position)
    {
        m_vec3Position = vec3Position;

        m_EntityUI.SetPosition(vec3Position);
    }

    public Vector3 GetPosition()
    {
        return m_vec3Position;
    }

    public void SetRotation(Vector3 vec3Rotation)
    {
        m_vec3Rotation = vec3Rotation;

        m_EntityUI.SetRotation(vec3Rotation);
    }

    public Vector3 GetRotation()
    {
        return m_vec3Rotation;
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
