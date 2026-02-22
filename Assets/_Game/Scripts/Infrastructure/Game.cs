using _Game.Scripts.Infrastructure.AssetManagement;
using _Game.Scripts.Infrastructure.States.ConcreteStates;
using Zenject;

namespace _Game.Scripts.Infrastructure
{
    public class Game : IInitializable
    {
        private readonly GameStateMachine _stateMachine;

        public Game(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}