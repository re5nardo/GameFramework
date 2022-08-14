using System.Collections;
using UnityEngine;

namespace GameFramework
{
    public abstract class MonoEnumerator : MonoBehaviour, IEnumerator
    {
        public object Current { get; protected set; }

        private bool isDone;

        private bool isSuccess;
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
}
