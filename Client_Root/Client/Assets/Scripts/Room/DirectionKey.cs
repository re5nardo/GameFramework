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
    public void OnHold()
    {
        Vector3 vec3Direction = UICamera.lastWorldPosition - m_trDirectionKey.position;
        Vector2 vec2Direction = new Vector2(vec3Direction.x, vec3Direction.y);
        vec2Direction.Normalize();

        if (onHold != null)
            onHold(vec2Direction);
    }
#endregion
}
