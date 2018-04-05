using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameUI : MonoBehaviour
{
    [SerializeField] UILabel m_lbNickname = null;

    private Transform m_trMine = null;
    private Transform m_trTarget = null;

    private void Awake()
    {
        m_trMine = transform;
    }

    public void SetData(Transform target, string strNickname, Color color)
    {
        m_trTarget = target;
        m_lbNickname.text = strNickname;
        m_lbNickname.color = color;
    }

    private void LateUpdate()
    {
        if (m_trTarget != null)
        {
            Vector3 vec3Pos = Camera.main.WorldToScreenPoint(m_trTarget.position);
            vec3Pos.z = 0;

            m_trMine.position = UICamera.mainCamera.ScreenToWorldPoint(vec3Pos);
        }
    }
}