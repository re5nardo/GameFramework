using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : IMonoTickUpdatable
{
    [SerializeField] private Vector3 m_vec3Start;
    [SerializeField] private Vector3 m_vec3End;
    [SerializeField] private float m_fSpeed = 3;
    [SerializeField] private float m_fInitValue = 0;

    private List<Rigidbody> m_dicCollision = new List<Rigidbody>();
    private Transform m_trMine;
    private float m_fElapsedTime;
    private float m_fExpectedTime;

    private void Awake()
    {
        BaeGameRoom2.Instance.RegisterMovingObject(this);

        m_trMine = transform;

        m_fExpectedTime = (m_vec3End - m_vec3Start).magnitude / m_fSpeed;
    }

    protected override void UpdateBody(int nUpdateTick)
    {
        m_fElapsedTime = nUpdateTick * BaeGameRoom2.Instance.GetTickInterval() + (m_fInitValue / 1 * m_fExpectedTime);

        float fTime = m_fElapsedTime % m_fExpectedTime;

        m_trMine.position = Vector3.Lerp(m_vec3Start, m_vec3End, fTime / m_fExpectedTime);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("[OnCollisionEnter] " + collisionInfo.gameObject.name);

//        m_dicCollision.Add(collisionInfo.rigidbody, transform.position);
    }
}
