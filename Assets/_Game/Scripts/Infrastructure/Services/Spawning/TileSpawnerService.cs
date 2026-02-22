using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.Infrastructure.Services.Score;
using _Game.Scripts.Infrastructure.Services.StaticData;
using _Game.Scripts.TileScripts;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace _Game.Scripts.Infrastructure.Services.Spawning
{
    public class TileSpawnerService : IInitializable
    {
        private readonly GameplayFactory _gameplayFactory;
        private readonly StaticDataService _staticData;
        private readonly ParticleService _particleService;
        private readonly AudioService _audioService;
        private readonly ScoreService _scoreService;

        private IObjectPool<TileCube> _pool;
        private Transform _poolContainer;

        public TileSpawnerService(
            GameplayFactory gameplayFactory,
            StaticDataService staticData,
            ParticleService particleService,
            AudioService audioService,
            ScoreService scoreService)
        {
            _gameplayFactory = gameplayFactory;
            _staticData = staticData;
            _particleService = particleService;
            _audioService = audioService;
            _scoreService = scoreService;
        }

        public void Initialize()
        {
            _poolContainer = new GameObject("[TilePool_Container]").transform;

            _pool = new ObjectPool<TileCube>(
                createFunc: CreateTile,
                actionOnGet: OnGetTile,
                actionOnRelease: tile => tile.gameObject.SetActive(false),
                actionOnDestroy: tile => Object.Destroy(tile.gameObject),
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 50
            );
        }

        private TileCube CreateTile()
        {
            return _gameplayFactory.CreateTile(_poolContainer);
        }

        private void OnGetTile(TileCube tile)
        {
            tile.gameObject.SetActive(true);

            if (tile.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public TileCube SpawnTile(Vector3 spawnPosition)
        {
            TileCube tile = _pool.Get();
            tile.transform.position = spawnPosition;
            tile.transform.rotation = Quaternion.identity;

            var config = _staticData.StaticDataContainer.TileContainer.Config;
            int value = Random.value < config.chanceForTwo ? 2 : 4;

            tile.Initialize(value, config, _pool, _particleService, _audioService, _gameplayFactory, _scoreService);

            return tile;
        }
    }
}