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

            protected void StartStateMachine()
            {
                CurrentState = InitState;
                CurrentState.Enter();
            }

            private void Update()
            {
                CurrentState?.Execute();
            }

            public IState MoveNext<I>(I input) where I : Enum
            {
                var next = CurrentState.GetNext(input);

                if (CurrentState == next)
                {
                    return CurrentState;
                }

                CurrentState.Exit();

                CurrentState = next;

                CurrentState.Enter();

                return CurrentState;
            }
        }
    }
}
