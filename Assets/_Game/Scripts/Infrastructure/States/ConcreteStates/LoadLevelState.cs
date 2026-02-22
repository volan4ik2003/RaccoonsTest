using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.Input;
using _Game.Scripts.Infrastructure.Services.Spawning;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.States.ConcreteStates
{
    public class LoadLevelState : IState
    {
        private readonly LazyInject<GameStateMachine> _gameStateMachine;
        private readonly GameplayFactory _gameplayFactory;
        
        public LoadLevelState(LazyInject<GameStateMachine> gameStateMachine, GameplayFactory gameplayFactory)
        {
            _gameStateMachine = gameStateMachine;
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