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
        private readonly UIFactory _uiFactory;
        
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
            var sceneContext = Object.FindAnyObjectByType<SceneContext>();

            if (sceneContext != null)
            {
                var spawner = sceneContext.Container.Resolve<TileSpawnerService>();
                var controller = sceneContext.Container.Resolve<TileControllerService>();
                var audio = sceneContext.Container.Resolve<AudioService>();

                spawner.InitPool();
                controller.StartGame();
                audio.Init();
            }
            else
            {
                Debug.LogError("No Scene Context");
            }

            _uiFactory.CreateHUD();
        }

        public void Exit()
        {
           
        }
    }
}