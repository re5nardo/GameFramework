using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Fast : MovingObject
{
	[SerializeField] private CollisionReporter m_CollisionReporter = null;

    public override void Initialize()
    {
        m_trMine = transform;

        m_vec3Start = new Vector3(Random.Range(35, 36), Random.Range(20, 1000), 0);
        m_vec3End = new Vector3(-m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);

		if(Random.Range(0, 2) == 0)
		{
			Vector3 temp = m_vec3Start;
			m_vec3Start = m_vec3End;
			m_vec3End = temp;
		}

        m_fSpeed = Random.Range(30, 40);
        m_fInitValue = Random.Range(0f, 1f);

        m_fExpectedTime = (m_vec3End - m_vec3Start).magnitude / m_fSpeed;
		m_fTickInterval = IGameRoom.Instance.GetTickInterval();

		m_bPredictPlay = true;
    }

    private void Start()
    {
		m_CollisionReporter.onCollisionEnter += OnCollisionEnter;
    }

    private void OnDestroy()
    {
		m_CollisionReporter.onTriggerEnter -= OnTriggerEnter;
    }
}