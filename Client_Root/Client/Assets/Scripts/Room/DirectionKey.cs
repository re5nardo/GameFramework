using UnityEngine;
using System.Collections;

public class DirectionKey : MonoBehaviour
{
    [HideInInspector] public Vector2Handler onHold = null;

    private Transform m_trDirectionKey = null;

    private Vector2 m_vec2Start;

    private void Start ()
    {
        m_trDirectionKey = transform;
	}

#region Event Handler
    public void OnHold(int nTouchID)
    {
        Vector2 touch = UICamera.currentCamera.ScreenToWorldPoint(UICamera.GetTouch(nTouchID).pos);
        Vector2 vec2Direction = new Vector2(touch.x - m_vec2Start.x, touch.y - m_vec2Start.y);
        vec2Direction.Normalize();

        if (onHold != null)
            onHold(vec2Direction);
    }

    public void OnPressd(int nTouchID)
    {
        m_vec2Start = UICamera.currentCamera.ScreenToWorldPoint(UICamera.GetTouch(nTouchID).pos);
    }
#endregion
}
