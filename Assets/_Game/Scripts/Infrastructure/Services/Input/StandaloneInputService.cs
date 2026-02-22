using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public override Vector2 Axis
        {
            get
            {
                if (IsPointerOverUI())
                    return Vector2.zero;

                return new Vector2(
                    UnityEngine.Input.GetAxisRaw(Horizontal),
                    UnityEngine.Input.GetAxisRaw(Vertical));
            }
        }

        public override bool FirePressed
        {
            get
            {
                if (IsPointerOverUI())
                    return false;

                return UnityEngine.Input.GetKeyDown(KeyCode.Space)
                       || UnityEngine.Input.GetMouseButtonDown(0);
            }
        }
    }
}