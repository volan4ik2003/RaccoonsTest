using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.AssetManagement
{
    public class AssetProvider : IService
    {
        public StaticDataContainer LoadStaticDataContainer()
        {
            var asset = Resources.Load<StaticDataContainer>(AssetPath.StaticDataContainerPath);
            return asset;
        }
    }
}