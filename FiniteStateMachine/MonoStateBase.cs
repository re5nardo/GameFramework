﻿using UnityEngine;
using System;

namespace GameFramework
{
    namespace FSM
    {
        public abstract class MonoStateBase : MonoBehaviour, IState
        {
            public IFiniteStateMachine FSM => gameObject.GetOrAddComponent<MonoStateMachineBase>();
            public virtual bool IsCurrent => FSM.CurrentState.Equals(this);

            public virtual void Enter()
            {
            }

            public virtual void Execute()
            {
            }

            public virtual void Exit()
            {
            }

            public abstract IState GetNext<I>(I input) where I : Enum;

            private void OnDestroy()
            {
                if (IsCurrent)
                {
                    Exit();
                }
            }
        }
    }
}