using UnityEngine;
using System.Collections;

public abstract class IBehaviorBasedObjectAI : MonoBehaviour
{
    private void Start()
    {
        IGameRoom.Instance.RegisterObstacle(this);
    }

    private void OnDestroy()
    {
        if(IGameRoom.GetInstance() != null)
            IGameRoom.Instance.UnRegisterObstacle(this);
    }

    public abstract void Init();

    public virtual void StartAI()
    {
        StartCoroutine(Do());
    }

    protected abstract IEnumerator Do();
}
