using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.StaticData;
using _Game.Scripts.TileScripts;
using _Game.Scripts.TileScripts.StaticData;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.Factories
{
    public class GameplayFactory : IService
    {
        private readonly StaticDataService _staticDataService;
        private readonly IInstantiator _instantiator;

        public GameplayFactory(StaticDataService staticDataService, IInstantiator instantiator)
        {
            _staticDataService = staticDataService;
            _instantiator = instantiator;
        }

        public void Init()
        {
            
        }

        public TileCube CreateTile(Transform parent)
        {
            TileCube prefab = _staticDataService.StaticDataContainer.TileContainer.TilePrefab;

            return _instantiator.InstantiatePrefabForComponent<TileCube>(prefab, parent);
        }
    }

}