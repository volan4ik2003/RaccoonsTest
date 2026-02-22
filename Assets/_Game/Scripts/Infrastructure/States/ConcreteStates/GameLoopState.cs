using _Game.Scripts.Infrastructure.Factories;
using Zenject;

namespace _Game.Scripts.Infrastructure.States.ConcreteStates
{
    public class GameLoopState : IState
    {
        private readonly LazyInject<GameStateMachine> _gameStateMachine;
        private readonly GameplayFactory _gameplayFactory;

        public GameLoopState(LazyInject<GameStateMachine> gameStateMachine, GameplayFactory gameplayFactory)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayFactory = gameplayFactory;
        }

        public void Enter()
        {
            _gameplayFactory.OnWinReached += OnWin;
        }

        public void Exit()
        {
            _gameplayFactory.OnWinReached -= OnWin;
        }

        private void OnWin()
        {
            _gameStateMachine.Value.Enter<WinGameState>();
        }
    }
}