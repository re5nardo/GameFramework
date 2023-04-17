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
                CurrentState.Enter();
            }

            public void StopStateMachine()
            {
                CurrentState?.Exit();
                CurrentState = null;
            }

            public void MoveNext<I>(I input) where I : Enum
            {
                var next = CurrentState.GetNext(input);

                if (CurrentState == next)
                {
                    Debug.LogWarning($"next is same with current. next: {next}");
                    return;
                }

                CurrentState.Exit();

                CurrentState = next;
                OnStateChange();

                CurrentState.Enter();
            }

            public virtual void OnStateChange() { }

            private void OnDestroy()
            {
                StopStateMachine();
            }
        }
    }
}
