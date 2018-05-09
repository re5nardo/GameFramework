using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [Header("[3D Camera]")]
    [SerializeField] private Transform m_trCamera = null;

    [Header("[2D UI]")]
    [SerializeField] private DirectionKey m_DirectionKey = null;
    [SerializeField] private UIToggle m_toggleFollowTarget = null;

    [Header("[Default Value]")]
	[SerializeField] private float m_fDefaultRotation_Y = 0;
    [SerializeField] private float m_fDefaultRotation_X = 10;
    [SerializeField] private float m_fDefaultZoomValue = -30;
    [SerializeField] private float m_fDefaultMoveSensitivity = 1;
    [SerializeField] private float m_fDefaultRotationSensitivity = 1;

    private const float ROTATION_X_MIN = 10;
    private const float ROTATION_X_MAX = 60;
    private const float ZOOM_VALUE_MIN = -30;
    private const float ZOOM_VALUE_MAX = -2;
    private const float MOVE_SENSITIVITY_MIN = 1;  
    private const float MOVE_SENSITIVITY_MAX = 5;
    private const float ROTATION_SENSITIVITY_MIN = 1;
    private const float ROTATION_SENSITIVITY_MAX = 5;

    private Transform m_trCameraController = null;
    private Transform m_trTarget = null;
    private bool m_bFollow = false;

    private float m_fUserRotation_Y_ = 0;
    private float m_fUserRotation_Y
    {
        get
        {
            return m_fUserRotation_Y_;
        }
        set
        {
            m_trCameraController.rotation = Quaternion.Euler(m_fUserRotation_X, value, 0);

            m_fUserRotation_Y_ = value;
        }
    }

    private float m_fUserRotation_X_ = 0;
    private float m_fUserRotation_X
    {
        get
        {
            return m_fUserRotation_X_;
        }
        set
        {
            float fValue = value > ROTATION_X_MAX ? ROTATION_X_MAX : value < ROTATION_X_MIN ? ROTATION_X_MIN : value;

            m_trCameraController.rotation = Quaternion.Euler(fValue, m_fUserRotation_Y, 0);

            m_fUserRotation_X_ = fValue;
        }
    }

    private float m_fUserZoomValue_ = 0;
    private float m_fUserZoomValue
    {
        get
        {
            return m_fUserZoomValue_;
        }
        set
        {
            float fValue = value > ZOOM_VALUE_MAX ? ZOOM_VALUE_MAX : value < ZOOM_VALUE_MIN ? ZOOM_VALUE_MIN : value;

            m_trCamera.localPosition = new Vector3(0, 0, fValue);

            m_fUserZoomValue_ = fValue;
        }
    }

    private float m_fUserMoveSensitivity_ = 0;
    private float m_fUserMoveSensitivity
    {
        get
        {
            return m_fUserMoveSensitivity_;
        }
        set
        {
            float fValue = value > MOVE_SENSITIVITY_MAX ? MOVE_SENSITIVITY_MAX : value < MOVE_SENSITIVITY_MIN ? MOVE_SENSITIVITY_MIN : value;

            m_fUserMoveSensitivity_ = fValue;
        }
    }

    private float m_fUserRotationSensitivity_ = 0;
    private float m_fUserRotationSensitivity
    {
        get
        {
            return m_fUserRotationSensitivity_;
        }
        set
        {
            float fValue = value > ROTATION_SENSITIVITY_MAX ? ROTATION_SENSITIVITY_MAX : value < ROTATION_SENSITIVITY_MIN ? ROTATION_SENSITIVITY_MIN : value;

            m_fUserRotationSensitivity_ = fValue;
        }
    }

#region MonoBehaviour
    private void Start()
    {
        m_trCameraController = transform;
        m_trCamera.parent = m_trCameraController;

        m_DirectionKey.onHold += OnDirectionKeyHold;

        m_fUserRotation_Y = m_fDefaultRotation_Y;
        m_fUserRotation_X = m_fDefaultRotation_X;
        m_fUserZoomValue = m_fDefaultZoomValue;
        m_fUserMoveSensitivity = m_fDefaultMoveSensitivity;
        m_fUserRotationSensitivity = m_fDefaultRotationSensitivity;
    }

    private void OnDestroy()
    {
        m_DirectionKey.onHold -= OnDirectionKeyHold;
    }

    private void LateUpdate()
    {
        if(m_bFollow)
        {
			m_trCameraController.position = new Vector3(0, m_trTarget.position.y, 0);
        }
    }
#endregion

    public void SetTarget(Transform trTarget)
    {
        m_trTarget = trTarget;
    }

    public void StartFollowTarget()
    {
        if (m_bFollow)
            return;
        
        m_bFollow = true;

        m_toggleFollowTarget.value = true;
    }

    public void StopFollowTarget()
    {
        if (!m_bFollow)
            return;
        
        m_bFollow = false;

        m_toggleFollowTarget.value = false;
    }

    private void SaveCameraSetting()
    {
        PlayerPrefs.SetFloat("UserRotation_Y", m_fUserRotation_Y);
        PlayerPrefs.SetFloat("UserRotation_X", m_fUserRotation_X);
        PlayerPrefs.SetFloat("UserZoomValue", m_fUserZoomValue);
        PlayerPrefs.SetFloat("UserMoveSensitivity", m_fUserMoveSensitivity);
        PlayerPrefs.SetFloat("UserRotationSensitivity", m_fUserRotationSensitivity);
    }

    private void LoadCameraSetting()
    {
        m_fUserRotation_Y = PlayerPrefs.HasKey("UserRotation_Y") ? PlayerPrefs.GetFloat("UserRotation_Y") : m_fDefaultRotation_Y;
        m_fUserRotation_X = PlayerPrefs.HasKey("UserRotation_X") ? PlayerPrefs.GetFloat("UserRotation_X") : m_fDefaultRotation_X;
        m_fUserZoomValue = PlayerPrefs.HasKey("UserZoomValue") ? PlayerPrefs.GetFloat("UserZoomValue") : m_fDefaultZoomValue;
        m_fUserMoveSensitivity = PlayerPrefs.HasKey("UserMoveSensitivity") ? PlayerPrefs.GetFloat("UserMoveSensitivity") : m_fDefaultMoveSensitivity;
        m_fUserRotationSensitivity = PlayerPrefs.HasKey("UserRotationSensitivity") ? PlayerPrefs.GetFloat("UserRotationSensitivity") : m_fDefaultRotationSensitivity;
    }

#region Event Handler
    public void OnGoToTargetBtnClicked()
    {
        if (m_trTarget == null)
        {
            Debug.LogError("m_trTarget is null!");
            return;
        }

        m_trCameraController.position = m_trTarget.position;
    }

    public void OnRotateLeftBtnClicked()
    {
        m_fUserRotation_Y -= m_fUserRotationSensitivity;
    }

    public void OnRotateRightBtnClicked()
    {
        m_fUserRotation_Y += m_fUserRotationSensitivity;
    }

    public void OnRotateUpBtnClicked()
    {
        m_fUserRotation_X += m_fUserRotationSensitivity;
    }

    public void OnRotateDownBtnClicked()
    {
        m_fUserRotation_X -= m_fUserRotationSensitivity;
    }

    public void OnZoomInBtnClicked()
    {
        m_fUserZoomValue++;
    }

    public void OnZoomOutBtnClicked()
    {
        m_fUserZoomValue--;
    }

    public void OnFollowTargetToggled(bool bValue)
    {
        if (bValue)
        {
            StartFollowTarget();
        }
        else
        {
            StopFollowTarget();
        }
    }

    private void OnDirectionKeyHold(Vector2 vec2Direction)
    {
        m_toggleFollowTarget.value = false;

        Vector3 vec3Direction = new Vector3(vec2Direction.x, 0, vec2Direction.y);
        vec3Direction.Normalize();

        Vector3 vec3Offset = Quaternion.Euler(0, m_fUserRotation_Y, 0) * vec3Direction * m_fUserMoveSensitivity;

        m_trCameraController.position += vec3Offset;
    }
#endregion
}
