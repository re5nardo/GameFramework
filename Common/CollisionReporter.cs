using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    public delegate void CollisionHandler(Collider collider, Collision collision);
    public delegate void ColliderHandler(Collider collider1, Collider collider2);

    [HideInInspector] public CollisionHandler onCollisionEnter;
    [HideInInspector] public CollisionHandler onCollisionStay;
    [HideInInspector] public CollisionHandler onCollisionExit;
    [HideInInspector] public ColliderHandler onTriggerEnter;
	[HideInInspector] public ColliderHandler onTriggerStay;
    [HideInInspector] public ColliderHandler onTriggerExit;

	private Collider colliderMine;

    private void Awake()
    {
        colliderMine = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(colliderMine, collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay?.Invoke(colliderMine, collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit?.Invoke(colliderMine, collision);
    }

	private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(colliderMine, other);
    }

	private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(colliderMine, other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(colliderMine, other);
    }
}
