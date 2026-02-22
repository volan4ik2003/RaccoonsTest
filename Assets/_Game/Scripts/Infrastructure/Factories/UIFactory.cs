using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Factories
{
    public class UIFactory : IService
    {
        public Transform UIRoot { get; private set; }

        private readonly StaticDataService _staticDataService;

        public UIFactory(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        public void Init()
        {
            
        }

    }
}