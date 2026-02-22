using _Game.Scripts.Infrastructure.AssetManagement;

namespace _Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IService
    {
        private readonly AssetProvider _assetProvider;
        public StaticDataContainer StaticDataContainer { get; set;}
      
        public StaticDataService(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public void Load()
        {
            StaticDataContainer = _assetProvider.LoadStaticDataContainer();  
        }
    }
}