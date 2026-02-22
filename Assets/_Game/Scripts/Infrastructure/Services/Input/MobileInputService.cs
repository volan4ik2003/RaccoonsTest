using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public sealed class MobileInputService : InputService
    {
        public override Vector2 Axis => _axis;
        public override bool FirePressed => _firePressed;

        private readonly ICoroutineRunner _coroutineRunner;

        private Vector2 _axis;
        private bool _firePressed;

        public MobileInputService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _coroutineRunner.StartCoroutine(UpdateInputCoroutine());
        }

        private IEnumerator UpdateInputCoroutine()
        {
            while (true)
            {
                ResetFrameState();

                if (!IsTouchAllowed())
                {
                    yield return null;
                    continue;
                }

                if (TryReadTouch(out Touch touch))
                    ProcessTouch(touch);

                yield return null;
            }
        }

        private void ResetFrameState()
        {
            _axis = Vector2.zero;
            _firePressed = false;
        }

        private bool IsTouchAllowed()
        {
            return !IsPointerOverUI();
        }

        private bool TryReadTouch(out Touch touch)
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                touch = default;
                return false;
            }

            touch = UnityEngine.Input.GetTouch(0);
            return true;
        }

        private void ProcessTouch(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _firePressed = true;
                    break;

                case TouchPhase.Moved:
                    UpdateAxis(touch);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _axis = Vector2.zero;
                    break;
            }
        }

        private void UpdateAxis(Touch touch)
        {
            _axis = touch.deltaPosition.normalized;
        }
    }
}
