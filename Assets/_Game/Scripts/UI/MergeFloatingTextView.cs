using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI
{
    public class MergeFloatingTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private bool _faceCamera = true;
        [SerializeField] private float _startYOffset = 1.15f;
        [SerializeField] private float _flyDistance = 1.25f;
        [SerializeField] private float _moveDuration = 0.9f;
        [SerializeField] private float _fadeDelay = 0.25f;
        [SerializeField] private float _fadeDuration = 0.55f;
        [SerializeField] private Ease _moveEase = Ease.OutCubic;
        [SerializeField] private Ease _fadeEase = Ease.InOutSine;
        [SerializeField] private bool _destroyOnComplete = true;

        private Sequence _sequence;

        private void Awake()
        {
            if (_text == null)
            {
                _text = GetComponentInChildren<TMP_Text>(true);
            }
        }

        private void OnDisable()
        {
            _sequence?.Kill();
            _sequence = null;
        }

        public void Play(string value, Color color)
        {
            if (_text == null)
            {
                Debug.LogWarning($"[{nameof(MergeFloatingTextView)}] TMP_Text reference is missing.", this);
                return;
            }

            _sequence?.Kill();

            transform.position += Vector3.up * _startYOffset;

            if (_faceCamera)
            {
                FaceCamera();
            }

            color.a = 1f;
            _text.text = "+" + value;
            _text.color = color;

            Color transparent = color;
            transparent.a = 0f;

            _sequence = DOTween.Sequence()
                .Join(transform
                    .DOMoveY(transform.position.y + _flyDistance, _moveDuration)
                    .SetEase(_moveEase))
                .Insert(_fadeDelay, DOTween
                    .To(() => _text.color, textColor => _text.color = textColor, transparent, _fadeDuration)
                    .SetEase(_fadeEase))
                .OnComplete(HandleCompleted)
                .SetLink(gameObject);
        }

        private void FaceCamera()
        {
            UnityEngine.Camera camera = UnityEngine.Camera.main;
            if (camera == null) return;

            transform.rotation = camera.transform.rotation;
        }

        private void HandleCompleted()
        {
            _sequence = null;

            if (_destroyOnComplete)
            {
                Destroy(gameObject);
            }
        }
    }
}
