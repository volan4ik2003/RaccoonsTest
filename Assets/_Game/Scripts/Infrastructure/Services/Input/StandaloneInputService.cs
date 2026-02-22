using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public class StandaloneInputService : IInputService
    {
        public Vector2 Axis
        {
            get
            {
                Vector2 axis = Vector2.zero;
                if (Keyboard.current != null)
                {
                    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                        axis.x = -1f;
                    else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                        axis.x = 1f;
                }
                return axis;
            }
        }

        public bool FirePressed
        {
            get
            {
                bool mouseClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
                bool spacePress = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

                return mouseClick || spacePress;
            }
        }
    }
}