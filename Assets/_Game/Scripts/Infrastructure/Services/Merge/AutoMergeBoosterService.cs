using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.Camera;
using _Game.Scripts.Infrastructure.Services.Input;
using _Game.Scripts.TileScripts;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services
{
    public class AutoMergeBoosterService : IService
    {
        private readonly ITileRegistry _registry;
        private readonly ITileMergeService _mergeService;
        private readonly IInputService _inputService;
        private readonly CameraService _cameraService;

        private const float RiseHeight = 4.5f;
        private const float WindUpDistance = 0.6f;
        private const float RiseDuration = 0.45f;
        private const float WindUpDuration = 0.25f;
        private const float StrikeDuration = 0.12f;

        private const float PopScaleFactor = 1.3f;
        private const float PopDuration = 0.15f;
        private const float ReturnDuration = 0.1f;

        public AutoMergeBoosterService(
            ITileRegistry registry,
            ITileMergeService mergeService,
            IInputService inputService,
            CameraService cameraService)
        {
            _registry = registry;
            _mergeService = mergeService;
            _inputService = inputService;
            _cameraService = cameraService;
        }

        public async UniTask<bool> ExecuteAsync(CancellationToken token)
        {
            var pair = FindPair();
            if (pair == null) return false;

            _inputService.IsBlocked = true;

            var (cubeA, cubeB) = pair.Value;
            cubeA.PrepareForBooster();
            cubeB.PrepareForBooster();

            await PlayMergeAnimationAsync(cubeA.transform, cubeB.transform, token);

            if (token.IsCancellationRequested)
            {
                _inputService.IsBlocked = false;
                return false;
            }

            cubeA.FinalizeBooster();
            cubeB.FinalizeBooster();

            Vector3 center = cubeA.transform.position;

            _mergeService.Merge(cubeA, cubeB, center);

            AnimatePopAsync(cubeA.transform, token).Forget();

            _cameraService.ShakeAsync(0.2f, 0.15f, 7, token).Forget();

            _inputService.IsBlocked = false;

            return true;
        }

        private async UniTaskVoid AnimatePopAsync(Transform target, CancellationToken token)
        {
            if (target == null) return;

            Vector3 originalScale = target.transform.localScale;
            Vector3 targetScale = originalScale * PopScaleFactor;

            float elapsed = 0f;

            while (elapsed < PopDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / PopDuration;

                if (target == null) return;

                target.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.SmoothStep(0f, 1f, t));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            elapsed = 0f;

            while (elapsed < ReturnDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / ReturnDuration;

                if (target == null) return;

                target.localScale = Vector3.Lerp(targetScale, originalScale, Mathf.SmoothStep(0f, 1f, t));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (target != null)
            {
                target.localScale = originalScale;
            }
        }

        private (TileCube, TileCube)? FindPair()
        {
            var group = _registry.Tiles
                .Where(c => c.IsFired) 
                .GroupBy(c => c.GetValue())
                .FirstOrDefault(g => g.Count() >= 2);

            if (group != null)
            {
                var list = group.ToList();
                return (list[0], list[1]);
            }
            return null;
        }

        private async UniTask PlayMergeAnimationAsync(Transform a, Transform b, CancellationToken token)
        {
            Vector3 startA = a.position;
            Vector3 startB = b.position;

            Vector3 highA = new Vector3(startA.x, RiseHeight, startA.z);
            Vector3 highB = new Vector3(startB.x, RiseHeight, startB.z);

            Vector3 centerHigh = new Vector3((startA.x + startB.x) / 2f, RiseHeight, (startA.z + startB.z) / 2f);

            Vector3 dirToA = (highA - highB).normalized;

            await UniTask.WhenAll(
                AnimateMoveAsync(a, startA, highA, RiseDuration, EaseOutBack, token),
                AnimateMoveAsync(b, startB, highB, RiseDuration, EaseOutBack, token)
            );

            await UniTask.WhenAll(
                AnimateMoveAsync(a, highA, highA + dirToA * WindUpDistance, WindUpDuration, EaseInOutSine, token),
                AnimateMoveAsync(b, highB, highB - dirToA * WindUpDistance, WindUpDuration, EaseInOutSine, token)
            );

            await UniTask.WhenAll(
                AnimateMoveAsync(a, a.position, centerHigh, StrikeDuration, EaseInExpo, token),
                AnimateMoveAsync(b, b.position, centerHigh, StrikeDuration, EaseInExpo, token)
            );
        }

        private async UniTask AnimateMoveAsync(Transform target, Vector3 from, Vector3 to, float duration, Func<float, float> easeFunc, CancellationToken token)
        {
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / duration);

                if (target != null)
                    target.position = Vector3.LerpUnclamped(from, to, easeFunc(t));

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (target != null)
                target.position = to;
        }

        private float EaseInOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
        private float EaseInExpo(float t) => t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
        private float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;

            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }
    }
}
