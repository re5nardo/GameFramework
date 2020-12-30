using System;

namespace GameFramework
{
    namespace FSM
    {
        public interface IFiniteStateMachine<S, I> where S : IState<I> where I : Enum
        {
            S InitState { get; }
            S CurrentState { get; }
            S MoveNext(I input);    //  인풋값에 따라 상태 전이
        }

        public interface IState<I> where I : Enum
        {
            IFiniteStateMachine<IState<I>, I> FSM { get; }

            void Enter();
            void Execute();
            void Exit();

            IState<I> GetNext(I input); //  인풋값에 의한 다음 상태 값 반환
        }
    }
}
