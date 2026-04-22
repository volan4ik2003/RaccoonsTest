using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.Camera;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.Infrastructure.Services.Score;
using _Game.Scripts.TileScripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services
{
    public class TileMergeService : ITileMergeService
    {
        private const float MergeShakeDuration = 0.42f;
        private const float MergeShakeMagnitude = 0.4f;
        private const float MergeShakeSpeed = 9f;

        private readonly GameplayFactory _gameplayFactory;
        private readonly ScoreService _scoreService;
        private readonly AudioService _audioService;
        private readonly ParticleService _particleService;
        private readonly CameraService _cameraService;
        private readonly MergeFloatingTextService _floatingTextService;

        public TileMergeService(
            GameplayFactory gameplayFactory,
            ScoreService scoreService,
            AudioService audioService,
            ParticleService particleService,
            CameraService cameraService,
            MergeFloatingTextService floatingTextService)
        {
            _gameplayFactory = gameplayFactory;
            _scoreService = scoreService;
            _audioService = audioService;
            _particleService = particleService;
            _cameraService = cameraService;
            _floatingTextService = floatingTextService;
        }

        public void Merge(TileCube main, TileCube other, Vector3 pos)
        {
            main.SetValue(main.GetValue() * 2);
            int mergedValue = main.GetValue();
            Color resultColor = main.tileRenderer.material.color;

            if (mergedValue == 2048)
            {
                _gameplayFactory.NotifyWin();
            }

            main.PlayMergeJump();
            main.PlayMergeBounce();
            _audioService.PlaySfx(SoundId.Merge);
            _cameraService.ShakeAsync(MergeShakeDuration, MergeShakeMagnitude, MergeShakeSpeed).Forget();
            _floatingTextService.Show(main.GetDisplayValue(), pos, resultColor);

            _particleService?.Play(
                ParticleId.TileHit,
                pos,
                resultColor
            );

            _scoreService.AddScore(mergedValue);

            other.ReturnToPool();

            main.ResetMerging();
        }
    }
}
