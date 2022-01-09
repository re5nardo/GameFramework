using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoEnumerator : MonoBehaviour, IEnumerator
{
    public object Current { get; protected set; }

    protected bool isDone;

    protected bool isSuccess;
    public bool IsSuccess
    {
        get => isSuccess;
        protected set
        {
            StopCoroutine("OnExecute");
            isSuccess = value;
            isDone = true;
        }
    }

    public bool MoveNext()
    {
        return !isDone;
    }

    public void Reset()
    {
        Current = null;
        isDone = false;
    }

    public MonoEnumerator Execute()
    {
        OnBeforeExecute();

        StartCoroutine("OnExecute");

        return this;
    }

    public virtual void OnBeforeExecute() { }
    public virtual IEnumerator OnExecute()
    {
        yield break;
    }
}
