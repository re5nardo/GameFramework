using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityUI : PooledComponent
{
    private GameObject m_goModel = null;
    private Animation m_animModel = null;

    private Transform m_trEntityUI = null;

    private void Awake()
    {
        m_trEntityUI = transform;
    }

    public void Initialize(FBS.Data.EntityType entityType, int nID, int nMasterDataID)
    {
        string strModelPath = "";

        if (entityType == FBS.Data.EntityType.Character)
        {
            MasterData.Character character = null;
            MasterDataManager.Instance.GetData<MasterData.Character>(nMasterDataID, ref character);

            strModelPath = string.Format("CharacterModel/{0}", character.m_strModelResName);
        }
        else if (entityType == FBS.Data.EntityType.Projectile)
        {
            MasterData.Projectile projectile = null;
            MasterDataManager.Instance.GetData<MasterData.Projectile>(nMasterDataID, ref projectile);

            strModelPath = string.Format("ProjectileModel/{0}", projectile.m_strModelResName);
        }

        m_goModel = ObjectPool.Instance.GetGameObject(strModelPath);
        m_animModel = m_goModel.GetComponent<Animation>();

        m_goModel.transform.parent = m_trEntityUI;
        m_goModel.transform.localPosition = Vector3.zero;
        m_goModel.transform.localRotation = Quaternion.identity;
        m_goModel.transform.localScale = Vector3.one;
    }

    public float GetAnimationClipLegth(string strClipName)
    {
        return m_animModel[strClipName].length;
    }

    public void SampleAnimation(string strClipName, float fNormalizedTime, float fWeight = 1f)
    {
        if (!IsClipNameValid(strClipName))
        {
            Debug.LogError("strClipName is invalid!, strClipName : " + strClipName);
            return;
        }

        m_animModel[strClipName].normalizedTime = fNormalizedTime;
        m_animModel[strClipName].enabled = true;
        m_animModel[strClipName].weight = fWeight;
        m_animModel.Sample();
        m_animModel[strClipName].enabled = false;
    }

    public float GetAnimationStateTime(string strClipName)
    {
        if (!IsClipNameValid(strClipName))
        {
            Debug.LogError("strClipName is invalid!, strClipName : " + strClipName);
            return 0f;
        }

        return m_animModel[strClipName].time;
    }

    private bool IsClipNameValid(string strClipName)
    {
        return m_animModel.GetClip(strClipName) != null;
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

            m_animModel[behavior.m_strAnimationName].time = kv.Value % m_animModel[behavior.m_strAnimationName].length;
            m_animModel[behavior.m_strAnimationName].enabled = true;
            m_animModel[behavior.m_strAnimationName].weight = 1;
        }

        m_animModel.Sample();

        foreach(KeyValuePair<int, float> kv in dicBehavior)
        {
            MasterData.Behavior behavior = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(kv.Key, ref behavior);

            m_animModel[behavior.m_strAnimationName].enabled = false;
        }
    }

    public void Clear()
    {
        if (m_goModel != null)
        {
            ObjectPool.Instance.ReturnGameObject(m_goModel);
            m_goModel = null;
        }
    }

    public override void OnReturned()
    {
        Clear();
    }
}
