using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.Input;
using _Game.Scripts.Infrastructure.Services.Score;
using _Game.Scripts.Infrastructure.Services.Spawning;
using Zenject;

namespace _Game.Scripts
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TileMergeCoordinator>().AsSingle();
            Container.Bind<ITileMergeService>().To<TileMergeService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileSpawnerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileControllerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().AsSingle();
            Container.Bind<ScoreService>().AsSingle();

            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();

        }
    }
}
