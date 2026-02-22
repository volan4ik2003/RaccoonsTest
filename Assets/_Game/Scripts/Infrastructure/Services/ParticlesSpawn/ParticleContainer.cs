using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.ParticlesSpawn
{
    [CreateAssetMenu(menuName = "Configs/ParticleContainer")]
    public class ParticleContainer : ScriptableObject
    {
        public List<ParticleEntry> Particles;

        public ParticleSystem Get(ParticleId id)
        {
            foreach (var entry in Particles)
            {
                if (entry.Id == id)
                    return entry.Prefab;
            }

            Debug.LogWarning($"Particle for {id} not found!");
            
            return null;
        }
    }
}