using UnityEngine;
using TMPro;
using Zenject;
using _Game.Scripts.Infrastructure.Services.Score;

namespace _Game.Scripts.UI
{
    public class MergeCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counterText;

        private ScoreService _scoreService;

        [Inject]
        public void Construct(ScoreService scoreService)
        {
            _scoreService = scoreService;

            _scoreService.OnMergeCountChanged += UpdateView;

            UpdateView(_scoreService.MergedCount);
        }

        private void UpdateView(int currentScore)
        {
            _counterText.text = $"Score: {currentScore}";
        }

        private void OnDestroy()
        {
            if (_scoreService != null)
            {
                _scoreService.OnMergeCountChanged -= UpdateView;
            }
        }
    }
}