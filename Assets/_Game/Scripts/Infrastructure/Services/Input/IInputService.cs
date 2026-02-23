using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Input
{
    public  interface IInputService : IService
    {
        Vector2 Axis { get; }
        bool FirePressed { get; }
        bool IsBlocked { get; set; }
    }
}

