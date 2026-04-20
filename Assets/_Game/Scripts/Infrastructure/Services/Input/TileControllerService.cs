using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.Camera;
using _Game.Scripts.Infrastructure.Services.Spawning;
using _Game.Scripts.Infrastructure.Services.StaticData;
using _Game.Scripts.TileScripts;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public class TileControllerService : IInitializable, ITickable
    {
        private const float LaunchFovIncrease = 3f;
        private const float LaunchFovPunchDuration = 0.1f;
        private const float LaunchFovReturnDuration = 0.9f;
        private const float LaunchFovReturnOvershoot = 0.25f;

        private readonly TileSpawnerService _spawnService;
        private readonly StaticDataService _staticData;
        private readonly AudioService _audioService;
        private readonly IInputService _inputService;
        private readonly CameraService _cameraService;

        private TileCube _currentTile;
        private Rigidbody _currentRb;
        private bool _isActive;
        private float _targetX;

        private readonly Vector3 _spawnPosition = new Vector3(0, 1f, -4f);

        public TileControllerService(
            TileSpawnerService spawnService,
            StaticDataService staticData,
            IInputService inputService,
            AudioService audioService,
            CameraService cameraService)
        {
            _spawnService = spawnService;
            _staticData = staticData;
            _inputService = inputService;
            _audioService = audioService;
            _cameraService = cameraService;
        }

        public void Initialize()
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
            _targetX = _spawnPosition.x;
        }

        private void HandleMovement()
        {
            var config = _staticData.StaticDataContainer.TileContainer.Config;

            _targetX += _inputService.Axis.x * config.inputSensitivity;
            _targetX = Mathf.Clamp(_targetX, -config.horizontalLimit, config.horizontalLimit);

            Vector3 currentPos = _currentTile.transform.position;
            float smoothedX = Mathf.Lerp(currentPos.x, _targetX, Time.deltaTime * config.moveSpeed);
            _currentTile.transform.position = new Vector3(smoothedX, currentPos.y, currentPos.z);

            float velocityX = (_targetX - currentPos.x);

            float tiltAngle = -velocityX * 20f;
            tiltAngle = Mathf.Clamp(tiltAngle, -35f, 35f);

            Quaternion targetRotation = Quaternion.Euler(0, 0, tiltAngle);
            _currentTile.transform.rotation = Quaternion.Lerp(_currentTile.transform.rotation, targetRotation, Time.deltaTime * 15f);
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

            _currentTile.transform.rotation = Quaternion.identity;

            _currentRb.angularVelocity = Vector3.zero;

            _currentTile.SetFired();

            _currentRb.AddForce(Vector3.forward * config.shootForce, ForceMode.Impulse);
            _cameraService.PlayLaunchFovAsync(
                LaunchFovIncrease,
                LaunchFovPunchDuration,
                LaunchFovReturnDuration,
                LaunchFovReturnOvershoot).Forget();

            _audioService.PlaySfx(SoundId.Swoosh);

            _currentTile = null;
            _currentRb = null;

            await Task.Delay(config.spawnDelayMs);

            SpawnNext();
        }
    }
}
