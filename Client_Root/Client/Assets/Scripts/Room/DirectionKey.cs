using UnityEngine;
using System.Collections;

public class DirectionKey : MonoBehaviour
{
    public Vector2Handler onHold = null;

    private Transform m_trDirectionKey = null;

    private void Start ()
    {
        m_trDirectionKey = transform;
	}

#region Event Handler
    public void OnHold(int nTouchID)
    {
        Vector2 touch = UICamera.currentCamera.ScreenToWorldPoint(UICamera.GetTouch(nTouchID).pos);
        Vector2 vec2Direction = new Vector2(touch.x - m_trDirectionKey.position.x, touch.y - m_trDirectionKey.position.y);
        vec2Direction.Normalize();

        if (onHold != null)
            onHold(vec2Direction);
    }
#endregion
}
