using _Game.Scripts.Infrastructure.Factories;
using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.Infrastructure.Services.Audio;
using _Game.Scripts.Infrastructure.Services.ParticlesSpawn;
using _Game.Scripts.Infrastructure.Services.Score;
using _Game.Scripts.TileScripts.Effects;
using _Game.Scripts.TileScripts.StaticData;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace _Game.Scripts.TileScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class TileCube : MonoBehaviour
    {
        [SerializeField] private TileFlickerEffect _flickerEffect;
        [SerializeField] private TailVFX _vfxSync;

        private TextMeshPro[] numberTexts;
        public MeshRenderer tileRenderer;

        private int _value;
        private TileConfig _config;
        private IObjectPool<TileCube> _pool;
        private Rigidbody _rb;

        private bool isMerging;

        public event Action<TileCube, TileCube, Vector3> OnMergeRequested;

        [Inject] private ITileRegistry _registry;

        public bool IsFired { get; private set; }

        private void Awake()
        {
            numberTexts = GetComponentsInChildren<TextMeshPro>();
            _rb = GetComponent<Rigidbody>();
        }

        public void Initialize(int value, TileConfig config, IObjectPool<TileCube> pool)
        {
            _value = value;
            _config = config;
            _pool = pool;

            isMerging = false;
            IsFired = false;

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
                    tileRenderer.material.color = tc.color;
                    if (_vfxSync != null)
                    {
                        _vfxSync.SyncColor(tc.color);
                    }
                    break;
                }
            }
        }

        private void OnEnable()
        {
            isMerging = false;
            _registry.Register(this);
        }

        private void OnDisable()
        {
            _registry.Unregister(this);
        }

        public int GetValue() => _value;

        public void ReturnToPool()
        {
            _pool.Release(this);
        }

        public void ResetMerging()
        {
            isMerging = false;
        }
        public void SetFired()
        {
            IsFired = true;
        }
        private void OnCollisionEnter(Collision collision)
        {
            TryMerge(collision);
        }

        public void SetValue(int newValue)
        {
            _value = newValue;
            UpdateVisual();
        }

        public void PrepareForBooster()
        {
            isMerging = true;

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.isKinematic = true;
            
            GetComponent<Collider>().enabled = false;
        }

        public void FinalizeBooster()
        {
            _rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;
        }

        public void PlayMergeJump()
        {
            _rb.AddForce(Vector3.up * _config.mergeJumpForce, ForceMode.Impulse);

            _flickerEffect?.PlayFlick();
        }

        private void TryMerge(Collision collision)
        {
            if (isMerging) return;

            TileCube other = collision.gameObject.GetComponent<TileCube>();
            if (other == null) return;
            if (other.GetValue() != _value) return;

            if (collision.relativeVelocity.magnitude < _config.minMergeImpulse)
                return;

            if (GetInstanceID() < other.GetInstanceID())
                return;

            isMerging = true;

            Vector3 contactPoint = collision.contacts[0].point;

            OnMergeRequested?.Invoke(this, other, contactPoint);
        }
    }
}