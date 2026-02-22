using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public sealed class MobileInputService : InputService
    {
        public MobileInputService(ICoroutineRunner coroutineRunner)
        {
        }

        public override Vector2 Axis
        {
            get
            {
                if (Touchscreen.current != null && Touchscreen.current.primaryTouch.isInProgress)
                {
                    Vector2 delta = Touchscreen.current.primaryTouch.delta.ReadValue();
                    return delta.normalized;
                }

                return Vector2.zero;
            }
        }

        public override bool FirePressed
        {
            get
            {
                if (Touchscreen.current != null)
                {
                    return Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;
                }

                return false;
            }
        }
    }
}