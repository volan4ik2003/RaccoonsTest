using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.StaticData
{
    [CreateAssetMenu(menuName = "Configs/Static Data Container")]
    public class StaticDataContainer : ScriptableObject
    {
        public TileContainer TileContainer;
        public ParticleContainer ParticleContainer;
        public AudioContainer AudioContainer;

        public HUD HUD;
    }
}