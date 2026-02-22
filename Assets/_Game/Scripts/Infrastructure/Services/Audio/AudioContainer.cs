using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioContainer", menuName = "Static Data/Audio Container")]
    public class AudioContainer : ScriptableObject
    {
        [Header("All Sounds")]
        public List<SoundConfig> Sounds;
    }
}