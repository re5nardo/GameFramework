using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaSetter : MonoBehaviour
{
    private Renderer[] m_Renderers;

    private float m_fLastValue = -1;
    public float m_fValue = -1;

    private void Awake()
    {
        m_Renderers = GetComponentsInChildren<Renderer>(true);
    }

    private void Update()
    {
        if (m_fLastValue != m_fValue)
        {
            foreach (Renderer renderer in m_Renderers)
            {
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, m_fValue);
            }

            m_fLastValue = m_fValue;
        }
    }
}
