using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityUI : PooledComponent
{
    private GameObject m_goModel = null;
    private Animation m_animModel = null;

    private Transform m_trModel = null;
    private Rigidbody m_ModelRigidbody = null;
    private TickBasedAnimationPlayer m_TickBasedAnimationPlayer = null;

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
        else if (entityType == FBS.Data.EntityType.Item)
        {
            MasterData.Item item = null;
            MasterDataManager.Instance.GetData<MasterData.Item>(nMasterDataID, ref item);

            strModelPath = string.Format("ItemModel/{0}", item.m_strModelResName);
        }

        m_goModel = ObjectPool.Instance.GetGameObject(strModelPath);
        m_animModel = m_goModel.GetComponentInChildren<Animation>();

        m_goModel.transform.parent = transform;
        m_goModel.transform.localPosition = Vector3.zero;
        m_goModel.transform.localRotation = Quaternion.identity;
        m_goModel.transform.localScale = Vector3.one;

        m_trModel = m_goModel.transform;
        m_ModelRigidbody = m_goModel.GetComponent<Rigidbody>();
        m_TickBasedAnimationPlayer = m_goModel.GetComponent<TickBasedAnimationPlayer>();
        m_TickBasedAnimationPlayer.SetTickInterval(BaeGameRoom2.Instance.GetTickInterval());
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

    public void Move(Vector3 vec3Motion)
    {
        Vector3 move = new Vector3(vec3Motion.x, 0, vec3Motion.z);

        m_ModelRigidbody.MovePosition(m_ModelRigidbody.position + move);
    }

    public void Jump()
    {
        m_ModelRigidbody.AddForce(Vector3.up * 500);
    }

    public bool IsGrounded()
    {
        return true;

//        return m_CharacterController.isGrounded;
    }

    //  Local position
    public Vector3 GetPosition()
    {
        return m_trModel.localPosition;
    }

    //  Local position
    public void SetPosition(Vector3 vec3Position)
    {
        m_trModel.localPosition = vec3Position;
    }

    //  Local rotation
    public Vector3 GetRotation()
    {
        return m_trModel.localRotation.eulerAngles;
    }

    //  Local rotation
    public void SetRotation(Vector3 vec3Rotation)
    {
        m_trModel.localRotation = Quaternion.Euler(vec3Rotation);
    }

    //  Local rotation
    public Vector3 GetForward()
    {
        return m_trModel.forward;
    }

    public void Play(string strAnimation, int nStartTick, bool bLoop = false, float fWeight = 1)
    {
        m_TickBasedAnimationPlayer.Play(strAnimation, nStartTick, bLoop, fWeight);
    }

    public void Stop(string strAnimation)
    {
        m_TickBasedAnimationPlayer.Stop(strAnimation);
    }

    public Transform GetModelTransform()
    {
        return m_trModel;
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

    public void Draw(int nTick)
    {
        m_TickBasedAnimationPlayer.Draw(nTick);
    }
}
