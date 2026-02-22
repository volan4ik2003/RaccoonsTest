using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.ParticlesSpawn
{
    public class AutoDespawn : MonoBehaviour
    {
        private float _timeToDespawn = 5f;
        private Coroutine _coroutine;

        private Action _returnToPoolAction;

        public void Configure(float time, Action returnAction)
        {
            _timeToDespawn = time;
            _returnToPoolAction = returnAction;
            RestartTimer();
        }

        private void OnEnable()
        {
            if (_returnToPoolAction != null)
            {
                RestartTimer();
            }
        }

        private void RestartTimer()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(DespawnAfterDuration(_timeToDespawn));
        }

        private IEnumerator DespawnAfterDuration(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (_returnToPoolAction != null)
            {
                _returnToPoolAction.Invoke();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}