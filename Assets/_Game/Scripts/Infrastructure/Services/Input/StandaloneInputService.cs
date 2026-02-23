using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override bool IsBlocked { get; set; }

        public override Vector2 Axis
        {
            get
            {
                if (IsBlocked) return Vector2.zero;

                Vector2 axis = Vector2.zero;

                if (Keyboard.current != null && !IsPointerOverUI())
                {
                    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                        axis.x -= 1f;
                    else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                        axis.x += 1f;
                }

                if (Mouse.current != null && Mouse.current.leftButton.isPressed && !IsPointerOverUI())
                {
                    axis.x += Mouse.current.delta.x.ReadValue() * 0.05f;
                }

                return axis;
            }
        }

        public override bool FirePressed
        {
            get
            {
                if (IsBlocked) return false;

                bool mouseClick = Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame;

                bool spacePress = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

                return (mouseClick && !IsPointerOverUI()) || spacePress;
            }
        }
    }
}