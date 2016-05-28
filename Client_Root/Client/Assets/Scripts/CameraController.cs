using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_trViewPivot = null;
    [SerializeField] private Transform m_trCamera = null;
    [SerializeField] private Vector3 m_vec3InitPos = Vector3.zero;
    [SerializeField] private Vector3 m_vec3InitRot = Vector3.zero;

    private Transform m_trTarget = null;
    private bool m_bFollow = false;

	private void Start ()
    {
        if (m_trCamera == null)
        {
            Debug.LogError("m_trCamera is null!");
            return;
        }

        m_trViewPivot.localPosition = Vector3.zero;
        m_trViewPivot.localRotation = Quaternion.identity;
        m_trViewPivot.localScale = Vector3.one;

        m_trCamera.parent = m_trViewPivot;
        m_trCamera.localPosition = m_vec3InitPos;
        m_trCamera.localRotation = Quaternion.Euler(m_vec3InitRot);
        m_trCamera.localScale = Vector3.one;
	}

    private void Update()
    {
        if (m_bFollow)
        {
            m_trViewPivot.localPosition = m_trTarget.localPosition;
        }
    }

    public void FollowTarget(Transform trTarget)
    {
        m_trTarget = trTarget;
        m_trViewPivot.localPosition = trTarget.localPosition;
       
        m_bFollow = true;
    }

    public void StopFollow()
    {
        m_bFollow = false;
    }
}
