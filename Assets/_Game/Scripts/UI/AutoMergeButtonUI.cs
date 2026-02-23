using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Zenject;
using _Game.Scripts.Infrastructure.Services;

namespace _Game.Scripts.UI
{
    public class AutoMergeButtonUI : MonoBehaviour
    {
        [SerializeField] private Button _boosterButton;

        private AutoMergeBoosterService _boosterService;

        [Inject]
        public void Construct(AutoMergeBoosterService boosterService)
        {
            _boosterService = boosterService;
        }

        private void Awake()
        {
            _boosterButton.onClick.AddListener(() => OnButtonClickedAsync().Forget());
        }

        private async UniTaskVoid OnButtonClickedAsync()
        {
            _boosterButton.interactable = false;
            var token = this.GetCancellationTokenOnDestroy();

            bool isSuccess = await _boosterService.ExecuteAsync(token);

            if (token.IsCancellationRequested) return;

            _boosterButton.interactable = true;
        }
    }
}