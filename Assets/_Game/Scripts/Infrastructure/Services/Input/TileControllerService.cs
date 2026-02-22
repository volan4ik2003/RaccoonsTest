using System.Threading.Tasks;
using _Game.Scripts.Infrastructure.Services.Spawning;
using _Game.Scripts.Infrastructure.Services.StaticData;
using _Game.Scripts.TileScripts;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public class TileControllerService : ITickable
    {
        private readonly TileSpawnerService _spawnService;
        private readonly StaticDataService _staticData;
        private readonly IInputService _inputService;

        private TileCube _currentTile;
        private Rigidbody _currentRb;
        private bool _isActive;

        private readonly Vector3 _spawnPosition = new Vector3(0, 1f, -4f);

        public TileControllerService(
            TileSpawnerService spawnService,
            StaticDataService staticData,
            IInputService inputService)
        {
            _spawnService = spawnService;
            _staticData = staticData;
            _inputService = inputService;
        }

        public void StartGame()
        {
            _isActive = true;
            SpawnNext();
        }

        public void Tick()
        {
            if (!_isActive || _currentTile == null) return;

            HandleMovement();
            HandleShooting();
        }

        private void SpawnNext()
        {
            _currentTile = _spawnService.SpawnTile(_spawnPosition);
            _currentRb = _currentTile.GetComponent<Rigidbody>();
            _currentRb.isKinematic = true;
        }

        private void HandleMovement()
        {
            if (Mathf.Approximately(_inputService.Axis.x, 0f)) return;

            var config = _staticData.StaticDataContainer.TileContainer.Config;

            Vector3 newPos = _currentTile.transform.position;

            newPos.x += _inputService.Axis.x * config.moveSpeed * Time.deltaTime;

            newPos.x = Mathf.Clamp(newPos.x, -config.horizontalLimit, config.horizontalLimit);

            _currentTile.transform.position = newPos;
        }

        private void HandleShooting()
        {
            if (_inputService.FirePressed)
            {
                ShootTile();
            }
        }

        private async void ShootTile()
        {
            var config = _staticData.StaticDataContainer.TileContainer.Config;

            _currentRb.isKinematic = false;
            _currentRb.AddForce(Vector3.forward * config.shootForce, ForceMode.Impulse);

            _currentTile = null;
            _currentRb = null;

            await Task.Delay(config.spawnDelayMs);

            SpawnNext();
        }
    }
}