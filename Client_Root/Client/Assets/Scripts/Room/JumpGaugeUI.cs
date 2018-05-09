using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGaugeUI : MonoBehaviour
{
	[SerializeField] UISlider m_slider = null;

    private Transform m_trMine = null;
	private Character m_target = null;
	private Vector3 m_vec3Offset;

    private void Awake()
    {
        m_trMine = transform;
    }

    public void SetData(Character target, Vector3 vec3Offset)
    {
		m_target = target;
		m_vec3Offset = vec3Offset;
    }

    private void LateUpdate()
    {
		if (m_target != null)
        {
			m_slider.value = (float)m_target.GetJumpCount() / (float)m_target.GetMaximumJumpCount();

			Vector3 vec3Pos = Camera.main.WorldToScreenPoint(m_target.GetPosition()) + m_vec3Offset;
            vec3Pos.z = 0;

			m_trMine.position = UICamera.mainCamera.ScreenToWorldPoint(vec3Pos);
        }
    }
}