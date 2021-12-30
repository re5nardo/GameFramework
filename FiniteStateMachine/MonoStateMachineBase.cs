using UnityEngine;
using System;

namespace GameFramework
{
    namespace FSM
    {
        public abstract class MonoStateMachineBase : MonoBehaviour, IFiniteStateMachine
        {
            public abstract IState InitState { get; }
            public IState CurrentState { get; protected set; }

            public void StartStateMachine()
            {
                CurrentState = InitState;
                CurrentState.OnEnter();
            }

            public void MoveNext<I>(I input) where I : Enum
            {
                var next = CurrentState.GetNext(input);

                if (CurrentState == next)
                {
                    return;
                }

                StopAllCoroutines();
                CurrentState.OnExit();

                CurrentState = next;
                OnStateChange();

                CurrentState.OnEnter();
                StartCoroutine(CurrentState.OnExecute());
            }

            public virtual void OnStateChange() { }
        }
    }
}
