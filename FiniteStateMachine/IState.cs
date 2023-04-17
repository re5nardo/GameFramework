using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    namespace FSM
    {
        public interface IState
        {
            IFiniteStateMachine FSM { get; }
            bool IsCurrent { get; }

            void Enter();
            void Exit();

            void OnEnter();
            IEnumerator OnExecute();
            void OnExit();

            IState GetNext<I>(I input) where I : Enum;
        }
    }
}
