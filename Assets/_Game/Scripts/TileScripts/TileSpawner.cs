using _Game.Scripts.Infrastructure.AssetManagement;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.TileScripts;
using _Game.Scripts.TileScripts.StaticData;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class TileSpawner : MonoBehaviour
{
    public TileConfig config;
    public TileCube tilePrefab;
    public Transform spawnPoint;

    private IObjectPool<TileCube> _pool;
    
    private Transform _poolContainer;

    private ParticleService _particleService;

    [Inject]
    private void Construct(ParticleService particleService)
    {
        _particleService = particleService;
    }

    private void Awake()
    {
        _poolContainer = new GameObject("TilePool").transform;

        _pool = new ObjectPool<TileCube>(
            createFunc: CreateTile,
            actionOnGet: OnGetTile,
            actionOnRelease: OnReleaseTile,
            actionOnDestroy: OnDestroyTile,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    private TileCube CreateTile()
    {
        return Instantiate(tilePrefab, spawnPoint.position, Quaternion.identity, _poolContainer);
    }

    private void OnGetTile(TileCube tile)
    {
        tile.transform.position = spawnPoint.position;
        tile.transform.rotation = Quaternion.identity;
        tile.gameObject.SetActive(true);

        if (tile.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnReleaseTile(TileCube tile)
    {
        tile.gameObject.SetActive(false);
    }

    private void OnDestroyTile(TileCube tile)
    {
        Destroy(tile.gameObject);
    }

    public TileCube SpawnTile()
    {
        TileCube tile = _pool.Get();
        int value = Random.value < config.chanceForTwo ? 2 : 4;

        tile.Initialize(value, config, _pool, _particleService);

        return tile;
    }
}