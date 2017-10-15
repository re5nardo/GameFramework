using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICountTimer : MonoBehaviour
{
    [SerializeField] private UILabel m_lbText = null;

    private bool m_bWork = false;
    private float m_fTime;
    private int m_nStartFrameIndex;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Work(float fTime)
    {
        if (m_bWork)
        {
            return;
        }

        m_bWork = true;
        m_fTime = fTime;
        m_nStartFrameIndex = Time.frameCount;
    }

    public void Stop()
    {
        m_bWork = false;
    }

	private void Update()
    {
        if (m_bWork)
        {
            if (m_nStartFrameIndex < Time.frameCount)
            {
                m_fTime = Mathf.Max(0, m_fTime - Time.deltaTime);
            }

            m_lbText.text = ((int)m_fTime).ToString();
        }
	}
}
