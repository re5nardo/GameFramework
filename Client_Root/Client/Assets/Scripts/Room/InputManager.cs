using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject m_goInputPlane = null;

    private const int INPUT_PLANE_LAYER = 8;
    private const float INPUT_THRESHOLD = 1f;

    private bool m_bWork = false;
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

        m_goInputPlane.SetActive(true);
        m_goInputPlane.layer = INPUT_PLANE_LAYER;
        m_goInputPlane.GetComponent<BoxCollider>().size = new Vector3(fWidth, 0f, fHeight);
    }

    public void Stop()
    {
        if (!m_bWork)
        {
            return;
        }

        m_bWork = false;
        m_Camera = null;
        m_OnClicked = null;

        m_goInputPlane.SetActive(false);
    }

    public void OnPressed()
    {
        if (!m_bWork)
        {
            return;
        }

        RaycastHit hitInfo;

        if (Physics.Raycast(m_Camera.ScreenPointToRay(UICamera.lastTouchPosition), out hitInfo, Mathf.Infinity, 1 << INPUT_PLANE_LAYER))
        {
            {
                if (m_OnClicked != null)
                {
                    m_OnClicked(hitInfo.point);
                }
            }
        }
    }
}
