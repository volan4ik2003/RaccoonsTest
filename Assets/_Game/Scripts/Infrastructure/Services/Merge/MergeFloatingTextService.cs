using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services
{
    public class MergeFloatingTextService : IService
    {
        private readonly MergeFloatingTextView _prefab;
        private bool _warnedMissingPrefab;

        public MergeFloatingTextService(MergeFloatingTextView prefab)
        {
            _prefab = prefab;
        }

        public void Show(string value, Vector3 position, Color color)
        {
            if (_prefab == null)
            {
                WarnMissingPrefab();
                return;
            }

            MergeFloatingTextView view = Object.Instantiate(_prefab, position, Quaternion.identity);
            view.Play(value, color);
        }

        private void WarnMissingPrefab()
        {
            if (_warnedMissingPrefab) return;

            Debug.LogWarning($"[{nameof(MergeFloatingTextService)}] Merge floating text prefab is not assigned in MainSceneInstaller.");
            _warnedMissingPrefab = true;
        }
    }
}
