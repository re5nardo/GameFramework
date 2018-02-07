using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : IMonoTickUpdatable
{
    [SerializeField] private Vector3 m_vec3Start;
    [SerializeField] private Vector3 m_vec3End;

    private Dictionary<Rigidbody, Vector3> m_dicRider = new Dictionary<Rigidbody, Vector3>();
    private Transform m_trMine;
    private float m_fExpectedTime = 5;
    private float m_fElapsedTime;
    private float m_fTickInterval;
    private int m_nStartTick = -1;

    private void Awake()
    {
        BaeGameRoom2.Instance.RegisterFloatingObject(this);

        m_trMine = transform;
        m_fExpectedTime = Random.Range(2, 5);
        m_fTickInterval = BaeGameRoom2.Instance.GetTickInterval();
    }

    public void StartTick(int nSTartTick)
    {
        m_nStartTick = nSTartTick;
    }

    protected override void UpdateBody(int nUpdateTick)
    {
        m_fElapsedTime = (nUpdateTick + 1) * m_fTickInterval;

        float fTime = m_fElapsedTime % (m_fExpectedTime * 2);

        if (fTime < m_fExpectedTime)
        {
            m_trMine.position = Vector3.Lerp(m_vec3Start, m_vec3End, fTime / m_fExpectedTime);
        }
        else
        {
            m_trMine.position = Vector3.Lerp(m_vec3End, m_vec3Start, (fTime - m_fExpectedTime) / m_fExpectedTime);
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        Vector3 offset = (transform.position - m_dicRider[collisionInfo.rigidbody]);

        collisionInfo.rigidbody.MovePosition(collisionInfo.transform.position + offset);

        m_dicRider[collisionInfo.rigidbody] = transform.position;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        m_dicRider.Add(collisionInfo.rigidbody, transform.position);
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        m_dicRider.Remove(collisionInfo.rigidbody);
    }
}
