using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : IMonoTickUpdatable
{
    public enum Type
    {
        Possession,
        Instant,
    }

    private GameItemManager m_GameItemManager;
    private int m_nID = -1;
    private int m_nMasterDataID = -1;
    private float m_fTickInterval = 0;
    private int m_nStartTick = -1;

    private GameObject m_goModel;

    private Type m_Type;
    private int m_nLifespan;
    private string m_strItem;

    private Vector3 m_vec3Start;
    private Vector3 m_vec3End;
    private float m_fSpeed;

	private Vector3 m_vec3SavedPosition;
	private Vector3 m_vec3SavedRotation;
	private Vector3 m_vec3SavedScale;

//    public void OnUsed()
//    {
//    }
//
//    public void OnReturned()
//    {
//        m_goModel.GetComponent<CollisionReporter>().onTriggerEnter = null;
//
//        ObjectPool.Instance.ReturnGameObject(m_goModel);
//    }

    public int GetID()
    {
        return m_nID;
    }

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    protected override void UpdateBody(int nUpdateTick)
    {
        Vector3 vec3Moved = (m_vec3End - m_vec3Start).normalized * m_fSpeed * m_fTickInterval;

        m_goModel.transform.position = m_goModel.transform.position + vec3Moved;

        if (m_nStartTick + m_nLifespan == nUpdateTick)
        {
            m_GameItemManager.OnGameItemEnd(this);

            ObjectPool.Instance.ReturnGameObject(m_goModel);
        }
    }

    public void Initialize(GameItemManager gameItemManager, int nID, int nMasterDataID, float fTickInterval, Vector3 vec3Start, Vector3 vec3End, float fSpeed)
    {
        m_GameItemManager = gameItemManager;
        m_nID = nID;
        m_nMasterDataID = nMasterDataID;
        m_fTickInterval = fTickInterval;
        m_vec3Start = vec3Start;
        m_vec3End = vec3End;
        m_fSpeed = fSpeed;

        MasterData.GameItem masterData = null;
        MasterDataManager.Instance.GetData<MasterData.GameItem>(m_nMasterDataID, ref masterData);

        m_Type = masterData.m_Type;
//        m_nLifespan = (int)(masterData.m_fLifespan / fTickInterval);
        m_nLifespan = 100000;
        m_strItem = masterData.m_strName;

        m_goModel = ObjectPool.Instance.GetGameObject("ItemModel/" + masterData.m_strModelResName);
        m_goModel.transform.SetParent(transform);
        m_goModel.transform.position = m_vec3Start;
        m_goModel.GetComponent<CollisionReporter>().onTriggerEnter = OnTriggerEnter;

		m_bPredictPlay = true;
    }

    public void StartTick(int nStartTick)
    {
        m_nStartTick = nStartTick;
    }

    private void OnTriggerEnter(Collider collider)
    {
		if(IGameRoom.Instance.IsPredictMode())
    		return;

        if (collider.gameObject.layer == GameObjectLayer.CHARACTER)
        {
            Character character = collider.gameObject.GetComponentInParent<Character>();

            if (!character.IsAlive())
                return;

            if (m_Type == Type.Possession)
            {
                m_GameItemManager.OnGameItemEnd(this);

                character.OnGetGameItem(this);

                ObjectPool.Instance.ReturnGameObject(m_goModel);
            }
            else if (m_Type == Type.Instant)
            {
                ObjectPool.Instance.ReturnGameObject(m_goModel);
            }
        }
    }

	public void Save()
	{
		m_vec3SavedPosition = m_goModel.transform.localPosition;
		m_vec3SavedRotation = m_goModel.transform.localRotation.eulerAngles;
		m_vec3SavedScale = m_goModel.transform.localScale;
	}

	public void Restore()
	{
		m_goModel.transform.localPosition = m_vec3SavedPosition;
		m_goModel.transform.localRotation = Quaternion.Euler(m_vec3SavedRotation);
		m_goModel.transform.localScale = m_vec3SavedScale;
	}
}