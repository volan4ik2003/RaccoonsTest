using _Game.Scripts.Infrastructure.Services;
using _Game.Scripts.TileScripts;
using System;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrastructure.Services
{
    public class TileMergeCoordinator : IInitializable, IDisposable
    {
        private readonly ITileRegistry _registry;
        private readonly ITileMergeService _mergeService;

        public TileMergeCoordinator(
            ITileRegistry registry,
            ITileMergeService mergeService)
        {
            _registry = registry;
            _mergeService = mergeService;
        }

        public void Initialize()
        {
            _registry.TileAdded += Subscribe;
            _registry.TileRemoved += Unsubscribe;
        }

        private void Subscribe(TileCube tile)
        {
            tile.OnMergeRequested += HandleMerge;
        }

        private void Unsubscribe(TileCube tile)
        {
            tile.OnMergeRequested -= HandleMerge;
        }

        private void HandleMerge(TileCube a, TileCube b, Vector3 pos)
        {
            _mergeService.Merge(a, b, pos);
        }

        public void Dispose()
        {
            _registry.TileAdded -= Subscribe;
            _registry.TileRemoved -= Unsubscribe;
        }
    }
}
