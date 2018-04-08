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
    private float m_fTickInterval;
    private int m_nStartTick = -1;

    public void Initialize()
    {
        m_trMine = transform;

        m_trMine.localScale = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));

        m_vec3Start = new Vector3(Random.Range(40, 45), Random.Range(20, 1000), 0);
        m_vec3End = new Vector3(-m_vec3Start.x, m_vec3Start.y, m_vec3Start.z);
        m_fSpeed = Random.Range(10, 50);
        m_fInitValue = Random.Range(0f, 1f);

        m_fExpectedTime = (m_vec3End - m_vec3Start).magnitude / m_fSpeed;
        m_fTickInterval = BaeGameRoom2.Instance.GetTickInterval();
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
        if (collisionInfo.gameObject.layer == GameObjectLayer.CHARACTER)
        {
            Character character = collisionInfo.gameObject.GetComponentInParent<Character>();

            if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Invincible))
                return;
            
            IState state = Factory.Instance.CreateState(MasterDataDefine.StateID.FAINT);
            state.Initialize(character, MasterDataDefine.StateID.FAINT, BaeGameRoom2.Instance.GetTickInterval());

            character.AddState(state, BaeGameRoom2.Instance.GetCurrentTick());

            state.StartTick(BaeGameRoom2.Instance.GetCurrentTick());
            state.UpdateTick(BaeGameRoom2.Instance.GetCurrentTick());
        }
    }

    private void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.layer == GameObjectLayer.CHARACTER)
        {
            Character character = colliderInfo.gameObject.GetComponentInParent<Character>();

            if (!character.IsAlive() || character.HasCoreState(CoreState.CoreState_Invincible))
                return;

            IState state = Factory.Instance.CreateState(MasterDataDefine.StateID.FAINT);
            state.Initialize(character, MasterDataDefine.StateID.FAINT, BaeGameRoom2.Instance.GetTickInterval());

            character.AddState(state, BaeGameRoom2.Instance.GetCurrentTick());

            state.StartTick(BaeGameRoom2.Instance.GetCurrentTick());
            state.UpdateTick(BaeGameRoom2.Instance.GetCurrentTick());
        }
    }
}