using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityUI : PooledComponent
{
    private GameObject m_goModel = null;
    private Animation m_animModel = null;
    private GameObject m_goNicknameUI = null;
	private GameObject m_goJumpGaugeUI = null;

    private Transform m_trModel = null;
	private Rigidbody m_ModelRigidbody = null;
    private TickBasedAnimationPlayer m_TickBasedAnimationPlayer = null;

	private Vector3 m_vec3SavedPosition;
	private Vector3 m_vec3SavedRotation;
	private Vector3 m_vec3SavedRigidbodyVelocity;
	private Vector3 m_vec3SavedRigidbodyAngularVelocity;

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
		m_TickBasedAnimationPlayer.SetTickInterval(IGameRoom.Instance.GetTickInterval());

        m_goNicknameUI = ObjectPool.Instance.GetGameObject("UI/NicknameUI");
		m_goNicknameUI.GetComponent<NicknameUI>().SetData(m_goModel.transform, "Entity" + nID.ToString(), IGameRoom.Instance.GetUserEntityID() == nID ? Color.green : Color.blue);

		if(IGameRoom.Instance.GetUserEntityID() == nID)
		{
			m_goJumpGaugeUI = ObjectPool.Instance.GetGameObject("UI/JumpGaugeUI");
			m_goJumpGaugeUI.GetComponent<JumpGaugeUI>().SetData(IGameRoom.Instance.GetCharacter(nID), new Vector3(0, -13, 0));
		}
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
		m_ModelRigidbody.velocity = new Vector3(m_ModelRigidbody.velocity.x, 0, m_ModelRigidbody.velocity.z);

		m_ModelRigidbody.AddForce(m_ModelRigidbody.mass * -Physics.gravity * 1.2f, ForceMode.Impulse);
    }

    public bool IsGrounded()
    {
        return true;

//        return m_CharacterController.isGrounded;
    }

	public Vector3 GetVelocity()
    {
		return m_ModelRigidbody.velocity;
    }

	public void SetVelocity(Vector3 vec3Velocity)
    {
		m_ModelRigidbody.velocity = vec3Velocity;
    }

    public Vector3 GetPosition()
    {
		return m_ModelRigidbody.position;
    }

    public void SetPosition(Vector3 vec3Position)
    {
		m_ModelRigidbody.MovePosition(vec3Position);
    }

    public Vector3 GetRotation()
    {
		return m_ModelRigidbody.rotation.eulerAngles;
    }

    public void SetRotation(Vector3 vec3Rotation)
    {
		m_ModelRigidbody.MoveRotation(Quaternion.Euler(vec3Rotation));
    }

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

		if (m_goNicknameUI != null)
        {
			ObjectPool.Instance.ReturnGameObject(m_goNicknameUI);
			m_goNicknameUI = null;
        }

		if (m_goJumpGaugeUI != null)
        {
			ObjectPool.Instance.ReturnGameObject(m_goJumpGaugeUI);
			m_goJumpGaugeUI = null;
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

	public void Save()
	{
		m_vec3SavedPosition = m_ModelRigidbody.position;
		m_vec3SavedRotation = m_ModelRigidbody.rotation.eulerAngles;
		m_vec3SavedRigidbodyVelocity = m_ModelRigidbody.velocity;
		m_vec3SavedRigidbodyAngularVelocity = m_ModelRigidbody.angularVelocity;
	}

	public void Restore()
	{
		m_ModelRigidbody.MovePosition(m_vec3SavedPosition);
		m_ModelRigidbody.MoveRotation(Quaternion.Euler(m_vec3SavedRotation));
		m_ModelRigidbody.velocity = m_vec3SavedRigidbodyVelocity;
		m_ModelRigidbody.angularVelocity = m_vec3SavedRigidbodyAngularVelocity;
	}
}