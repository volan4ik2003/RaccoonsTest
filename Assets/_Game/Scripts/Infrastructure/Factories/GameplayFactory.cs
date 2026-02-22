using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.StaticData;
using _Game.Scripts.TileScripts;
using _Game.Scripts.TileScripts.StaticData;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Factories
{
    public class GameplayFactory : IService
    {
        private readonly StaticDataService _staticDataService;

        public GameplayFactory(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Init()
        {
            
        }

        public TileCube SpawnTile(GameObject tilePrefab, Transform spawnPoint, TileConfig tileConfig)
        {
            GameObject go = Object.Instantiate(tilePrefab, spawnPoint.position, Quaternion.identity);
            TileCube tile = go.GetComponent<TileCube>();

            // Определяем значение тайла по шансам
            int value = Random.value < tileConfig.chanceForTwo ? 2 : 4;

            // Инициализируем тайл с конфигом
            //tile.Initialize(value, tileConfig);

            return tile;
        }
    }

}