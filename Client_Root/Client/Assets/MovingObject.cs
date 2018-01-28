using System.Collections;
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

    private void Awake()
    {
        BaeGameRoom2.Instance.RegisterMovingObject(this);

        m_trMine = transform;

        m_trMine.localScale = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));

        m_vec3Start = new Vector3(Random.Range(40, 45), Random.Range(20, 1000), 0);
        m_vec3End = new Vector3(-m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);
        m_fSpeed = Random.Range(10, 50);
        m_fInitValue = Random.Range(0f, 1f);

        m_fExpectedTime = (m_vec3End - m_vec3Start).magnitude / m_fSpeed;
    }

    protected override void UpdateBody(int nUpdateTick)
    {
        m_fElapsedTime = nUpdateTick * BaeGameRoom2.Instance.GetTickInterval() + (m_fInitValue / 1 * m_fExpectedTime);

        float fTime = m_fElapsedTime % m_fExpectedTime;

        m_trMine.position = Vector3.Lerp(m_vec3Start, m_vec3End, fTime / m_fExpectedTime);
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
    }

    private void OnTriggerEnter(Collider colliderInfo)
    {
    }
}
