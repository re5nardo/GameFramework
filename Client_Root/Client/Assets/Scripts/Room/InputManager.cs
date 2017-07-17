using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const int INPUT_PLANE_LAYER = 8;
    private const float INPUT_THRESHOLD = 1f;

    private bool m_bWork = false;
    private GameObject m_goInputPlane = null;
    private Vector3Handler m_OnClicked = null;
    private Vector3 m_vec3InputDownPos = Vector3.zero;
    private Camera m_Camera = null;

    public void Work(float fWidth, float fHeight, Camera camera, Vector3Handler OnClicked)
    {
        if (m_bWork)
        {
            return;
        }

        m_bWork = true;
        m_Camera = camera;
        m_OnClicked = OnClicked;

        m_goInputPlane = new GameObject("InputPlane");
        m_goInputPlane.layer = INPUT_PLANE_LAYER;
        m_goInputPlane.AddComponent<BoxCollider>().size = new Vector3(fWidth, 0f, fHeight);
    }

    public void Stop()
    {
        if (!m_bWork)
        {
            return;
        }

        m_bWork = false;

        Destroy(m_goInputPlane);
    }

    private void Update()
    {
        if (!m_bWork)
        {
            return;
        }

        if(UICamera.hoveredObject != UICamera.fallThrough)
        {
            return;
        }

        RaycastHit hitInfo;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1 << INPUT_PLANE_LAYER))
            {
                m_vec3InputDownPos = hitInfo.point;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1 << INPUT_PLANE_LAYER))
            {
//                if (Vector3.Distance(m_vec3InputDownPos, hitInfo.point) <= INPUT_THRESHOLD)
                {
                    if (m_OnClicked != null)
                    {
                        m_OnClicked(hitInfo.point);
                    }
                }
            }
        }
    }
}
