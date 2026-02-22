using _Game.Scripts.Infrastructure.Factories;
using Zenject;

namespace _Game.Scripts.Infrastructure.States.ConcreteStates
{
    public class GameLoopState : IState
    {
        private readonly LazyInject<GameStateMachine> _gameStateMachine;
        private GameplayFactory _gameplayFactory;
        
        public GameLoopState(LazyInject<GameStateMachine> gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
          
        }
        
        public void Exit()
        {
          
        }
    }
}