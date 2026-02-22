using System;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Audio
{
    [Serializable]
    public class SoundConfig
    {
        public SoundId Id;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
    }
}