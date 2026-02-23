using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Camera
{
    public class CameraService : IService
    {
        private Transform _cameraTransform;
        private Vector3 _originalPosition;
        private CancellationTokenSource _shakeCts;

        public CameraService()
        {
            if (UnityEngine.Camera.main != null)
            {
                _cameraTransform = UnityEngine.Camera.main.transform;
                _originalPosition = _cameraTransform.localPosition;
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
    }
}