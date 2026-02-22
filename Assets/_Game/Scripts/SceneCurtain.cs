using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class SceneCurtain : MonoBehaviour
    {
        public static SceneCurtain Instance { get; private set; }

        [SerializeField] private Image _curtainImage;
        [SerializeField] private float _fadeDuration = 0.5f;
        public event Action OnFadeInComplete;
        public event Action OnFadeOutComplete;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void FadeIn(Action onComplete = null)
        {
            _curtainImage.raycastTarget = true;
            _curtainImage.DOFade(1f, _fadeDuration)
                .SetUpdate(true) 
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    OnFadeInComplete?.Invoke();
                });
        }

        public void FadeOut(Action onComplete = null)
        {
            _curtainImage.DOFade(0f, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _curtainImage.raycastTarget = false;
                    onComplete?.Invoke();
                    OnFadeOutComplete?.Invoke();
                });
        }

        public void InstantShow()
        {
            _curtainImage.color = new Color(0, 0, 0, 1f);
            _curtainImage.raycastTarget = true;
        }

        public void InstantHide()
        {
            _curtainImage.color = new Color(0, 0, 0, 0f);
            _curtainImage.raycastTarget = false;
        }
    }
}