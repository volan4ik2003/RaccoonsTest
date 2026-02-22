using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Game.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private Ease _easeType = Ease.OutQuart;

        private void Awake()
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Open()
        {
            gameObject.SetActive(true);

            _canvasGroup.alpha = 0f;

            _canvasGroup.DOFade(1f, _fadeDuration)
                .SetEase(_easeType)
                .SetUpdate(true);
        }

        public void Close()
        {
            _canvasGroup.DOFade(0f, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}