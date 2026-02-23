using _Game.Scripts.TileScripts;
using System;
using System.Collections.Generic;

namespace _Game.Scripts.Infrastructure.Services
{
    public class TileRegistry : ITileRegistry
    {
        private readonly List<TileCube> _tiles = new();

        public IReadOnlyList<TileCube> Tiles => _tiles;

        public event Action<TileCube> TileAdded;
        public event Action<TileCube> TileRemoved;

        public void Register(TileCube tile)
        {
            if (_tiles.Contains(tile)) return;

            _tiles.Add(tile);
            TileAdded?.Invoke(tile);
        }

        public void Unregister(TileCube tile)
        {
            if (_tiles.Remove(tile))
                TileRemoved?.Invoke(tile);
        }
    }
}