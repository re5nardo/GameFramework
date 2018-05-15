using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    [HideInInspector] public CollisionHandler onCollisionEnter;
    [HideInInspector] public ColliderHandler onTriggerEnter;

    private void OnCollisionEnter(Collision collision)
    {
		if(IGameRoom.Instance.IsPredictMode())
    		return;

        if (onCollisionEnter != null)
        {
            onCollisionEnter(collision);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
		if(IGameRoom.Instance.IsPredictMode())
    		return;

        if (onTriggerEnter != null)
        {
            onTriggerEnter(collider);
        }
    }
}