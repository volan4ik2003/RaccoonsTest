using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public sealed class MobileInputService : InputService
    {
        public override bool IsBlocked { get; set; }

        private bool _startedOverUI;

        public MobileInputService(ICoroutineRunner coroutineRunner)
        {
        }

        public override Vector2 Axis
        {
            get
            {
                if (IsBlocked) return Vector2.zero;

                if (_startedOverUI) return Vector2.zero;

                if (Touchscreen.current != null && Touchscreen.current.primaryTouch.isInProgress)
                {
                    Vector2 delta = Touchscreen.current.primaryTouch.delta.ReadValue();

                    return new Vector2((delta.x / Screen.width) * 10f, 0);
                }

                return Vector2.zero;
            }
        }

        public override bool FirePressed
        {
            get
            {
                if (IsBlocked) return false;

                if (Touchscreen.current != null)
                {
                    var touch = Touchscreen.current.primaryTouch;

                    if (touch.press.wasPressedThisFrame)
                    {
                        _startedOverUI = IsPointerOverUI();
                    }

                    if (touch.press.wasReleasedThisFrame)
                    {
                        return !_startedOverUI;
                    }
                }

                return false;
            }
        }
    }
}