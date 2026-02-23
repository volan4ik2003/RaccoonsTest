using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        public abstract Vector2 Axis { get; }
        public abstract bool FirePressed { get; }
        public abstract bool IsBlocked { get; set; }


        protected bool IsPointerOverUI()
        {
            if (EventSystem.current == null || Pointer.current == null)
                return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Pointer.current.position.ReadValue()
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.layer == 5)
                    return true;
            }

            return false;
        }
    }
}