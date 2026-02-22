using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.Pool;

namespace _Game.Scripts.Infrastructure.Services.ParticlesSpawn
{
    public sealed class ParticleService : IService
    {
        private readonly StaticDataService _staticData;
        
        private readonly Dictionary<ParticleId, IObjectPool<ParticleSystem>> _pools = new();
        
        private readonly Transform _particlesContainer;

        public ParticleService(StaticDataService staticData)
        {
            _staticData = staticData;
            _particlesContainer = new GameObject("[ParticlePool_Container]").transform;
        }

        public void Play(ParticleId id, Vector3 position, Color? color = null, float? scale = null, float? despawnAfter = null, Quaternion? rotation = null)
        {
            var pool = GetPool(id);
            var instance = pool.Get();

            instance.transform.SetPositionAndRotation(position, rotation ?? Quaternion.identity);

            if (color.HasValue)
            {
                Color targetColor = color.Value;
                targetColor.a = 1f;

                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(targetColor, 0.0f), new GradientColorKey(targetColor, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                );

                ApplyGradient(instance, gradient, targetColor);

                var children = instance.GetComponentsInChildren<ParticleSystem>();
                foreach (var child in children)
                {
                    ApplyGradient(child, gradient, targetColor);
                }
            }

            ApplyScale(instance, scale);

            instance.Play();

            ScheduleDespawn(instance, pool, despawnAfter);
        }

        private void ApplyGradient(ParticleSystem ps, Gradient grad, Color startCol)
        {
            var main = ps.main;
            main.startColor = startCol;

            var colorModule = ps.colorOverLifetime;
            if (colorModule.enabled)
            {
                colorModule.color = grad;
            }
        }

        private IObjectPool<ParticleSystem> GetPool(ParticleId id)
        {
            if (_pools.TryGetValue(id, out var existingPool))
                return existingPool;

            var prefab = _staticData.StaticDataContainer.ParticleContainer.Get(id);
            if (prefab == null)
            {
                Debug.LogError($"[ParticleService] Prefab for ID {id} not found!");
                return null;
            }

            var newPool = new ObjectPool<ParticleSystem>(
                createFunc: () => Object.Instantiate(prefab, _particlesContainer),
                actionOnGet: instance => instance.gameObject.SetActive(true),
                actionOnRelease: instance => instance.gameObject.SetActive(false),
                actionOnDestroy: instance => Object.Destroy(instance.gameObject),
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 100
            );

            _pools[id] = newPool;
            return newPool;
        }

        private void ApplyScale(ParticleSystem instance, float? scale)
        {
            if (!scale.HasValue) return;
            instance.transform.localScale = Vector3.one * scale.Value;
        }

        private void ScheduleDespawn(ParticleSystem instance, IObjectPool<ParticleSystem> pool, float? despawnAfter)
        {
            float duration = despawnAfter ?? instance.main.duration + instance.main.startLifetime.constantMax;

            if (instance.TryGetComponent(out AutoDespawn autoDespawn))
            {
                autoDespawn.Configure(duration, () => pool.Release(instance));
            }
            else
            {
                Debug.LogWarning($"[ParticleService] No script {instance.name} AutoDespawn!");
            }
        }
    }
}