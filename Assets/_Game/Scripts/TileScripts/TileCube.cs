using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.TileScripts.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace _Game.Scripts.TileScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class TileCube : MonoBehaviour
    {
        private TextMeshPro[] numberTexts;
        public MeshRenderer tileRenderer;

        private ParticleService _particleService;

        private int _value;
        private TileConfig _config;
        private IObjectPool<TileCube> _pool;
        private Rigidbody _rb;
        
        [SerializeField] private float minMergeImpulse = 2f; 

        public bool isMerging; 

        private void Awake()
        {
            numberTexts = GetComponentsInChildren<TextMeshPro>();
            _rb = GetComponent<Rigidbody>();
        }

        public void Initialize(int value, TileConfig config, IObjectPool<TileCube> pool, ParticleService particleService)
        {
            _value = value;
            _config = config;
            _pool = pool;
            _particleService = particleService;
            isMerging = false;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (TextMeshPro number in numberTexts)
            {
                number.text = _value.ToString();
            }

            foreach (var tc in _config.tileColors)
            {
                if (tc.number == _value)
                {
                    tileRenderer.material.color = tc.color * 0.5f;
                    break;
                }
            }
        }

        public int GetValue() => _value;

        public void ReturnToPool()
        {
            _pool.Release(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            TryMerge(collision);
        }

        private void TryMerge(Collision collision)
        {
            if (isMerging) return;

            TileCube other = collision.gameObject.GetComponent<TileCube>();
            if (other == null) return;
            if (other.isMerging) return;
            if (other.GetValue() != _value) return;

            if (collision.relativeVelocity.magnitude < minMergeImpulse) 
                return;

            if (gameObject.GetInstanceID() > other.gameObject.GetInstanceID())
            {
                isMerging = true;
                other.isMerging = true;

                Vector3 contactPoint = collision.contacts[0].point;

                MergeWith(other, contactPoint);
            }
        }

        private void MergeWith(TileCube other, Vector3 mergePosition)
        {
            _value *= 2;
            UpdateVisual();

            // ¡≈–≈Ã —»À” œ–€∆ ¿ »«  ŒÕ‘»√¿
            _rb.AddForce(Vector3.up * _config.mergeJumpForce, ForceMode.Impulse);

            if (_particleService != null)
            {
                _particleService.Play(ParticleId.TileHit, mergePosition);
            }

            other.ReturnToPool();
            isMerging = false;
        }
    }
}