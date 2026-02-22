using System.Collections.Generic;
using _Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.Services.Audio
{
    public class AudioService
    {
        private readonly StaticDataService _staticData;

        private AudioSource _sfxSource;
        private Dictionary<SoundId, SoundConfig> _soundCache;

        public AudioService(StaticDataService staticData)
        {
            _staticData = staticData;
        }

        public void Init()
        {
            if (_staticData.StaticDataContainer == null)
            {
                Debug.LogError("[AudioService] StaticDataContainer not loaded");
                return;
            }

            var audioContainer = _staticData.StaticDataContainer.AudioContainer;
            if (audioContainer == null)
            {
                Debug.LogError("[AudioService] No AudioContainer");
                return;
            }

            var audioObject = new GameObject("[AudioService_SFX]");
            Object.DontDestroyOnLoad(audioObject);
            _sfxSource = audioObject.AddComponent<AudioSource>();

            _soundCache = new Dictionary<SoundId, SoundConfig>();

            if (audioContainer.Sounds == null || audioContainer.Sounds.Count == 0)
            {
                Debug.LogWarning("[AudioService] Empty list!");
                return;
            }

            foreach (var sound in audioContainer.Sounds)
            {
                if (sound.Clip != null && !_soundCache.ContainsKey(sound.Id))
                {
                    _soundCache.Add(sound.Id, sound);
                }
            }
        }

        public void PlaySfx(SoundId soundId)
        {
            if (_soundCache.TryGetValue(soundId, out SoundConfig config))
            {
                _sfxSource.PlayOneShot(config.Clip, config.Volume);
            }
            else
            {
                Debug.LogWarning($"[AudioService] Sound {soundId} not found in StaticDataContainer!");
            }
        }
    }
}