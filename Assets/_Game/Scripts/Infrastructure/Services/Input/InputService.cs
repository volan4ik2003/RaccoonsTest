using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        public abstract Vector2 Axis { get; }
        public abstract bool FirePressed { get; }

        protected bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

#if UNITY_EDITOR || UNITY_STANDALONE
            return EventSystem.current.IsPointerOverGameObject();
#else
            if (UnityEngine.Input.touchCount == 0)
                return false;

            return EventSystem.current.IsPointerOverGameObject(
                UnityEngine.Input.GetTouch(0).fingerId);
#endif
        }
    }
}