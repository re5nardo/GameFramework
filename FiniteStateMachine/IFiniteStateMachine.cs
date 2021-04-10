﻿using System;

namespace GameFramework
{
    namespace FSM
    {
        public interface IFiniteStateMachine
        {
            IState InitState { get; }
            IState CurrentState { get; }
            IState MoveNext<I>(I input) where I : Enum;    //  인풋값에 따라 상태 전이
        }

        public interface IState
        {
            IFiniteStateMachine FSM { get; }
            bool IsCurrent { get; }

            void Enter();
            void Execute();
            void Exit();

            IState GetNext<I>(I input) where I : Enum; //  인풋값에 의한 다음 상태 값 반환
        }
    }
}
