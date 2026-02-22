using _Game.Scripts.Infrastructure;
using _Game.Scripts.Infrastructure.AssetManagement;
using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.Input;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.Infrastructure.Services.StaticData;
using _Game.Scripts.Infrastructure.States;
using _Game.Scripts.Infrastructure.States.ConcreteStates;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            // Core
            Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();

            // States
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();

            // Services
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.Bind<IInputService>()
                .FromMethod(ctx =>
                    InputServiceFactory.Create(
                        ctx.Container.Resolve<ICoroutineRunner>()))
                .AsSingle();
            Container.BindInterfacesAndSelfTo<ParticleService>().AsSingle();

            // Factories
            Container.BindInterfacesAndSelfTo<GameplayFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();

            // StateMachine
            Container.Bind<GameStateMachine>().AsSingle();

            // Game entry point
            Container.BindInterfacesAndSelfTo<Game>().AsSingle().NonLazy();
        }
    }
}
