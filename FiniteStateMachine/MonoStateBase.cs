using UnityEngine;
using System;
using System.Collections;

namespace GameFramework
{
    namespace FSM
    {
        public abstract class MonoStateBase : MonoBehaviour, IState
        {
            public IFiniteStateMachine FSM => gameObject.GetOrAddComponent<MonoStateMachineBase>();
            public virtual bool IsCurrent => FSM.CurrentState.Equals(this);

            public virtual void OnEnter() { }

            public virtual IEnumerator OnExecute()
            {
                yield break;
            }

            public virtual void OnExit() { }

            public abstract IState GetNext<I>(I input) where I : Enum;

            private void OnDestroy()
            {
                if (IsCurrent)
                {
                    OnExit();
                }
            }
        }
    }
}
