using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                Vector2 axis = Vector2.zero;

                if (Keyboard.current != null)
                {
                    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                        axis.x -= 1f;
                    else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                        axis.x += 1f;
                }

                if (Mouse.current != null && Mouse.current.leftButton.isPressed)
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
                bool mouseClick = Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame;

                bool spacePress = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

                return (mouseClick && !IsPointerOverUI()) || spacePress;
            }
        }
    }
}