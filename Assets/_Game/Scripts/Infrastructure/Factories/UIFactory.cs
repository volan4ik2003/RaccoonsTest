using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Zenject;

namespace _Game.Scripts.Infrastructure.Factories
{
    public class UIFactory : IInitializable, IService
    {
        private readonly StaticDataService _staticData;
        private readonly DiContainer _container;

        public HUD HUD { get; private set; }
        public Transform UIRoot { get; private set; }

        public UIFactory(StaticDataService staticData, DiContainer container)
        {
            _staticData = staticData;
            _container = container;
        }

        public void Initialize()
        {
            UIRoot = GameplaySceneContainer.Instance.UIRoot;
            CreateHUD();
        }

        public HUD CreateHUD()
        {
            HUD = _container.InstantiatePrefabForComponent<HUD>(
                _staticData.StaticDataContainer.HUD,
                UIRoot
            );

            return HUD;
        }

    }
}