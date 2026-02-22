using _Game.Scripts.Infrastructure.Services.StaticData;
using Zenject;

namespace _Game.Scripts.Infrastructure.States.ConcreteStates
{
    public class LoadProgressState : IState
    {
        private readonly LazyInject<GameStateMachine> _gameStateMachine;
        private readonly StaticDataService _staticData;
        
        public LoadProgressState(LazyInject<GameStateMachine> gameStateMachine,
            StaticDataService staticDataService)
        {
            _staticData = staticDataService;
            _gameStateMachine = gameStateMachine;
        }
        public void Enter()
        {
            LoadProgressOrInitNew();
            
            SceneLoader.Instance.LoadScene(StartupSceneTracker.SceneToLoadAfterBoot, () =>
            {
                switch (StartupSceneTracker.SceneToLoadAfterBoot)
                {
                    case SceneNames.MainScene:
                        _gameStateMachine.Value.Enter<LoadLevelState>();
                        break;
                }
            });
        }

        private void LoadProgressOrInitNew()
        {
            _staticData.Load();
        }

        public void Exit()
        {
            
        }
    }
}