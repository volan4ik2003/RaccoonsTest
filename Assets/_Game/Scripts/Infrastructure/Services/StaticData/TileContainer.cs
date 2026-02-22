using UnityEngine;
using _Game.Scripts.TileScripts;
using _Game.Scripts.TileScripts.StaticData;

namespace _Game.Scripts.Infrastructure.Services.StaticData
{
    [CreateAssetMenu(fileName = "TileContainer", menuName = "Static Data/Tile Container")]
    public class TileContainer : ScriptableObject
    {
        public TileCube TilePrefab;
        public TileConfig Config;
    }
}