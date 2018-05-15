﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : IMonoTickUpdatable
{
    private Vector3 m_vec3Start;
    private Vector3 m_vec3End;
    private float m_fSpeed;
    private float m_fInitValue;
    private List<Rigidbody> m_dicCollision = new List<Rigidbody>();
    private Transform m_trMine;
    private float m_fElapsedTime;
    private float m_fExpectedTime;
    private float m_fTickInterval;
    private int m_nStartTick = -1;

	private Vector3 m_vec3SavedPosition;
	private Vector3 m_vec3SavedRotation;
	private Vector3 m_vec3SavedScale;

    public void Initialize()
    {
        m_trMine = transform;

        m_trMine.localScale = new Vector3(Random.Range(2, 4), Random.Range(2, 3), Random.Range(2, 3));

        m_vec3Start = new Vector3(Random.Range(35, 36), Random.Range(20, 1000), 0);
        m_vec3End = new Vector3(-m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);

		if(Random.Range(0, 2) == 0)
		{
			Vector3 temp = m_vec3Start;
			m_vec3Start = m_vec3End;
			m_vec3End = temp;
		}

        m_fSpeed = Random.Range(10, 25);
        m_fInitValue = Random.Range(0f, 1f);

        m_fExpectedTime = (m_vec3End - m_vec3Start).magnitude / m_fSpeed;
		m_fTickInterval = IGameRoom.Instance.GetTickInterval();

		m_bPredictPlay = true;
    }

    public void StartTick(int nSTartTick)
    {
        m_nStartTick = nSTartTick;
    }

    protected override void UpdateBody(int nUpdateTick)
    {
        m_fElapsedTime = (nUpdateTick + 1) * m_fTickInterval + (m_fInitValue / 1 * m_fExpectedTime);

        float fTime = m_fElapsedTime % m_fExpectedTime;

        m_trMine.position = Vector3.Lerp(m_vec3Start, m_vec3End, fTime / m_fExpectedTime);
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
		if(IGameRoom.Instance.IsPredictMode())
    		return;

        if (collisionInfo.gameObject.layer == GameObjectLayer.CHARACTER)
        {
            Character character = collisionInfo.gameObject.GetComponentInParent<Character>();

            if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Invincible))
                return;

//            BaeGameRoom2.Instance.EntityAttack(-1, character.GetID(), 1);

			character.OnAttacked(-1, 1, IGameRoom.Instance.GetCurrentTick());
        }
    }

    private void OnTriggerEnter(Collider colliderInfo)
    {
		if(IGameRoom.Instance.IsPredictMode())
    		return;

        if (colliderInfo.gameObject.layer == GameObjectLayer.CHARACTER)
        {
            Character character = colliderInfo.gameObject.GetComponentInParent<Character>();

            if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Invincible))
                return;

//            BaeGameRoom2.Instance.EntityAttack(-1, character.GetID(), 1);

			character.OnAttacked(-1, 1, IGameRoom.Instance.GetCurrentTick());
        }
    }

	public void Save()
	{
		m_vec3SavedPosition = m_trMine.localPosition;
		m_vec3SavedRotation = m_trMine.localRotation.eulerAngles;
		m_vec3SavedScale = m_trMine.localScale;
	}

	public void Restore()
	{
		m_trMine.localPosition = m_vec3SavedPosition;
		m_trMine.localRotation = Quaternion.Euler(m_vec3SavedRotation);
		m_trMine.localScale = m_vec3SavedScale;
	}
}