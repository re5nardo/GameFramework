using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IEntityUI : MonoBehaviour
{
    [SerializeField] private Animation m_animEntityUI = null;

    private Transform m_trEntityUI = null;

    protected virtual void Awake()
    {
        m_trEntityUI = transform;
    }

    public float GetAnimationClipLegth(string strClipName)
    {
        return m_animEntityUI[strClipName].length;
    }

    public void SampleAnimation(string strClipName, float fNormalizedTime, float fWeight = 1f)
    {
        if (!IsClipNameValid(strClipName))
        {
            Debug.LogError("strClipName is invalid!, strClipName : " + strClipName);
            return;
        }

        m_animEntityUI[strClipName].normalizedTime = fNormalizedTime;
        m_animEntityUI[strClipName].enabled = true;
        m_animEntityUI[strClipName].weight = fWeight;
        m_animEntityUI.Sample();
        m_animEntityUI[strClipName].enabled = false;
    }

    public float GetAnimationStateTime(string strClipName)
    {
        if (!IsClipNameValid(strClipName))
        {
            Debug.LogError("strClipName is invalid!, strClipName : " + strClipName);
            return 0f;
        }

        return m_animEntityUI[strClipName].time;
    }

    private bool IsClipNameValid(string strClipName)
    {
        return m_animEntityUI.GetClip(strClipName) != null;
    }

    //  Local position
    public void SetPosition(Vector3 vec3Position)
    {
        m_trEntityUI.localPosition = vec3Position;
    }

    //  Local rotation
    public void SetRotation(Vector3 vec3Rotation)
    {
        m_trEntityUI.localRotation = Quaternion.Euler(vec3Rotation);
    }

    public void Sample(Dictionary<int, float> dicBehavior)
    {
        foreach(KeyValuePair<int, float> kv in dicBehavior)
        {
            MasterData.Behavior behavior = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(kv.Key, ref behavior);

            m_animEntityUI[behavior.m_strAnimationName].time = kv.Value % m_animEntityUI[behavior.m_strAnimationName].length;
            m_animEntityUI[behavior.m_strAnimationName].enabled = true;
            m_animEntityUI[behavior.m_strAnimationName].weight = 1;
        }

        m_animEntityUI.Sample();

        foreach(KeyValuePair<int, float> kv in dicBehavior)
        {
            MasterData.Behavior behavior = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(kv.Key, ref behavior);

            m_animEntityUI[behavior.m_strAnimationName].enabled = false;
        }
    }
}
