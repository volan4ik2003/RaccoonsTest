namespace _Game.Scripts.Infrastructure.Services.Input
{
    using UnityEngine;

    public static class InputServiceFactory
    {
        public static IInputService Create(ICoroutineRunner coroutineRunner)
        {
            return Application.platform switch
            {
                RuntimePlatform.Android or RuntimePlatform.IPhonePlayer => new MobileInputService(coroutineRunner),
                _ => new StandaloneInputService()
            };
        }
    }

}