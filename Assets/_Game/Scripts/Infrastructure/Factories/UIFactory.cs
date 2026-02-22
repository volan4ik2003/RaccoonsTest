using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace _Game.Scripts.Infrastructure.Factories
{
    public class UIFactory : IService
    {
        private readonly StaticDataService _staticData;
        public HUD HUD { get; private set; }
        public Transform UIRoot { get; private set; }

        public UIFactory(StaticDataService staticData)
        {
            _staticData = staticData;
        }

        public void Init()
        {
            UIRoot = GameplaySceneContainer.Instance.UIRoot;
        }

        public HUD CreateHUD()
        {
            HUD = Object.Instantiate(
                _staticData.StaticDataContainer.HUD,
                UIRoot
            );

            HUD.Init();
            return HUD;
        }

    }
}