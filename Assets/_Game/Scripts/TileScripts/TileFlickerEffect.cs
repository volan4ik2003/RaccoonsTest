using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.TileScripts.Effects
{
    [DisallowMultipleComponent]
    public class TileFlickerEffect : MonoBehaviour
    {
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
        private const string EMISSION_KEYWORD = "_EMISSION";

        [SerializeField] private Renderer[] _renderers;

        [Header("Flick Settings")]
        [SerializeField] private Color _flickColor = Color.white;
        [SerializeField, Min(0f)] private float _flickIntensity = 2f;
        [SerializeField, Min(0.01f)] private float _flickUp = 0.05f;
        [SerializeField, Min(0.01f)] private float _flickDown = 0.15f;

        [SerializeField] private Ease _flickUpEase = Ease.OutQuad;
        [SerializeField] private Ease _flickDownEase = Ease.InQuad;

        private MaterialPropertyBlock _mpb;
        private Sequence _flickTween;
        private Color _emissionBase = Color.black;

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();

            foreach (var r in _renderers)
            {
                if (r != null && r.sharedMaterial != null)
                {
                    r.sharedMaterial.EnableKeyword(EMISSION_KEYWORD);
                }
            }
        }

        private void OnDisable()
        {
            ResetFlick();
        }

        public void PlayFlick()
        {
            _flickTween?.Kill();

            float peak = Mathf.Max(0f, _flickIntensity);

            _flickTween = DOTween.Sequence()
                .Append(DOTween.To(SetEmission, 0f, peak, _flickUp).SetEase(_flickUpEase))
                .Append(DOTween.To(SetEmission, peak, 0f, _flickDown).SetEase(_flickDownEase))
                .OnComplete(ResetFlick);
        }

        public void ResetFlick()
        {
            _flickTween?.Kill();
            SetEmissionForAll(_emissionBase);
        }

        private void SetEmission(float intensity)
        {
            Color target = _emissionBase + (_flickColor * intensity);
            SetEmissionForAll(target);
        }

        private void SetEmissionForAll(Color color)
        {
            if (_renderers == null) return;

            foreach (var r in _renderers)
            {
                if (r == null) continue;

                r.GetPropertyBlock(_mpb);
                _mpb.SetColor(EmissionColorId, color);
                r.SetPropertyBlock(_mpb);
            }
        }
    }
}