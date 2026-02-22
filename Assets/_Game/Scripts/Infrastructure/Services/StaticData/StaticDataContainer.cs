using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.TileScripts.StaticData;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.StaticData
{
    [CreateAssetMenu(menuName = "Configs/Static Data Container")]
    public class StaticDataContainer : ScriptableObject
    {
        public TileContainer TileContainer;
        public ParticleContainer ParticleContainer;
    }
}