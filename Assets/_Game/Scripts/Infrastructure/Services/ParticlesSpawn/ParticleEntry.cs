using System;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.ParticlesSpawn
{
    [Serializable]
    public class ParticleEntry
    {
        public ParticleId Id;
        public ParticleSystem Prefab;
    }
}