using _Game.Scripts.TileScripts;
using UnityEngine;

namespace _Game.Scripts.Infrastructure.Services
{
    public interface ITileMergeService : IService
    {
        void Merge(TileCube main, TileCube other, Vector3 mergePosition);
    }
}