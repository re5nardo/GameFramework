using System;
using System.Collections;

namespace GameFramework
{
    namespace FSM
    {
        public interface IFiniteStateMachine
        {
            IState InitState { get; }
            IState CurrentState { get; }
            void MoveNext<I>(I input) where I : Enum;
            void OnStateChange();
        }
    }
}
