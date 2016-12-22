using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MisterBaeObstacleAI : IBehaviorBasedObjectAI
{
    [SerializeField] private float m_fStartDelay = 0f;

    [Header("Patrol")]
    [SerializeField] private Vector3 m_vec3PatrolStart = Vector3.zero;
    [SerializeField] private Vector3 m_vec3PatrolDest = Vector3.zero;

    private MisterBae m_MisterBae = null;

    public override void Init()
    {
        GameObject goCharacter = new GameObject("MisterBaeAI");
        m_MisterBae = goCharacter.AddComponent<MisterBae>();

        Stat stat = new Stat();
        stat.fSpeed = 4f;

        m_MisterBae.Initialize(stat);
    }


    protected override IEnumerator Do()
    {
        yield return new WaitForSeconds(m_fStartDelay);

        m_MisterBae.Patrol(m_vec3PatrolStart, m_vec3PatrolDest);
    }
}
