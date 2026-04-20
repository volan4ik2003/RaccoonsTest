using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Camera
{
    public class CameraService : IService
    {
        private UnityEngine.Camera _camera;
        private Transform _cameraTransform;
        private Vector3 _originalPosition;
        private float _originalFieldOfView;
        private CancellationTokenSource _shakeCts;
        private CancellationTokenSource _fovCts;

        public CameraService()
        {
            _camera = UnityEngine.Camera.main;

            if (_camera != null)
            {
                _cameraTransform = _camera.transform;
                _originalPosition = _cameraTransform.localPosition;
                _originalFieldOfView = _camera.fieldOfView;
            }
        }

        public async UniTask ShakeAsync(float duration, float magnitude, float speed = 25f, CancellationToken token = default)
        {
            if (_cameraTransform == null) return;

            _shakeCts?.Cancel();
            _shakeCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var linkedToken = _shakeCts.Token;

            float elapsed = 0f;

            float randomStart = UnityEngine.Random.Range(-1000f, 1000f);

            try
            {
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;

                    float fadeOut = 1f - (elapsed / duration);

                    float x = (Mathf.PerlinNoise(randomStart + elapsed * speed, 0f) * 2f - 1f) * magnitude * fadeOut;
                    float y = (Mathf.PerlinNoise(0f, randomStart + elapsed * speed) * 2f - 1f) * magnitude * fadeOut;

                    _cameraTransform.localPosition = new Vector3(
                        _originalPosition.x + x,
                        _originalPosition.y + y,
                        _originalPosition.z);

                    await UniTask.Yield(PlayerLoopTiming.Update, linkedToken);
                }
            }
            finally
            {
                if (_cameraTransform != null)
                {
                    _cameraTransform.localPosition = _originalPosition;
                }
            }
        }

        public async UniTask PlayLaunchFovAsync(
            float fovIncrease,
            float punchDuration,
            float returnDuration,
            float returnOvershoot,
            CancellationToken token = default)
        {
            if (_camera == null) return;

            _fovCts?.Cancel();

            var fovCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            _fovCts = fovCts;
            var linkedToken = fovCts.Token;

            float startFov = _camera.fieldOfView;
            float targetFov = _originalFieldOfView + fovIncrease;

            try
            {
                await AnimateFieldOfViewAsync(startFov, targetFov, punchDuration, EaseOutCubic, linkedToken);
                await AnimateFieldOfViewAsync(
                    _camera.fieldOfView,
                    _originalFieldOfView,
                    returnDuration,
                    t => EaseOutElastic(t, returnOvershoot),
                    linkedToken);
            }
            catch (System.OperationCanceledException) when (linkedToken.IsCancellationRequested)
            {
            }
            finally
            {
                if (_camera != null && ReferenceEquals(_fovCts, fovCts))
                {
                    _camera.fieldOfView = _originalFieldOfView;
                }
            }
        }

        private async UniTask AnimateFieldOfViewAsync(
            float from,
            float to,
            float duration,
            System.Func<float, float> ease,
            CancellationToken token)
        {
            if (duration <= 0f)
            {
                _camera.fieldOfView = to;
                return;
            }

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                if (_camera != null)
                {
                    _camera.fieldOfView = Mathf.LerpUnclamped(from, to, ease(t));
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (_camera != null)
            {
                _camera.fieldOfView = to;
            }
        }

        private static float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }

        private static float EaseOutElastic(float t, float overshoot)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;

            const float defaultElasticOvershoot = 0.37f;
            const float c4 = 2f * Mathf.PI / 3f;

            float elastic = Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
            float smooth = EaseOutCubic(t);
            float blend = Mathf.Clamp01(overshoot / defaultElasticOvershoot);

            return Mathf.LerpUnclamped(smooth, elastic, blend);
        }
    }
}
