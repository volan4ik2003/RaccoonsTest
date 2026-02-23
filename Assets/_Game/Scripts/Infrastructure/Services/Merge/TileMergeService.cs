using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.Infrastructure.Services.Score;
using _Game.Scripts.TileScripts;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services
{
    public class TileMergeService : ITileMergeService
    {
        private readonly GameplayFactory _gameplayFactory;
        private readonly ScoreService _scoreService;
        private readonly AudioService _audioService;
        private readonly ParticleService _particleService;

        public TileMergeService(
            GameplayFactory gameplayFactory,
            ScoreService scoreService,
            AudioService audioService,
            ParticleService particleService)
        {
            _gameplayFactory = gameplayFactory;
            _scoreService = scoreService;
            _audioService = audioService;
            _particleService = particleService;
        }

        public void Merge(TileCube main, TileCube other, Vector3 pos)
        {
            main.SetValue(main.GetValue() * 2);

            if (main.GetValue() == 2048)
            {
                _gameplayFactory.NotifyWin();
            }

            main.PlayMergeJump();
            _audioService.PlaySfx(SoundId.Merge);

            _particleService?.Play(
                ParticleId.TileHit,
                pos,
                main.tileRenderer.material.color
            );

            _scoreService.AddMerge();

            other.ReturnToPool();

            main.ResetMerging();
        }
    }
}