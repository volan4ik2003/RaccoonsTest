using _Game.Scripts.TileScripts;
using System;
using System.Collections.Generic;

namespace _Game.Scripts.Infrastructure.Services
{
    public interface ITileRegistry
    {
        IReadOnlyList<TileCube> Tiles { get; }

        event Action<TileCube> TileAdded;
        event Action<TileCube> TileRemoved;

        void Register(TileCube tile);
        void Unregister(TileCube tile);
    }
}
