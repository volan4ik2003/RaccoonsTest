using _Game.Scripts.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.States.ConcreteStates
{
    public class LoadLevelState : IState
    {
        private readonly LazyInject<GameStateMachine> _gameStateMachine;
        private readonly GameplayFactory _gameplayFactory;
        private readonly UIFactory _uiFactory;
        private Vector3 _spawnPoint;
        
        public LoadLevelState(LazyInject<GameStateMachine> gameStateMachine, GameplayFactory gameplayFactory, UIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
        }

        public void Enter()
        {
            SceneCurtain.Instance.FadeIn(() =>
            {
                SceneLoader.Instance.LoadScene(SceneNames.MainScene, SceneLoaded);
            });
        }

        private void SceneLoaded()
        {
            SceneCurtain.Instance.FadeOut();

            _gameplayFactory.Init();
            _uiFactory.Init();
            
            InitGameWorld();
            
            _gameStateMachine.Value.Enter<GameLoopState>();
        }
        
        private void InitGameWorld()
        {
        }
        
        public void Exit()
        {
           
        }
    }
}