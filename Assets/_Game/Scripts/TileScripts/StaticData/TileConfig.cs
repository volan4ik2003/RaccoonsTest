using UnityEngine;

namespace _Game.Scripts.TileScripts.StaticData
{
    [CreateAssetMenu(
        fileName = "TileConfig",
        menuName = "Game/TileCube/TileCube Config"
    )]
    public class TileConfig : ScriptableObject
    {
        [Header("Tile Movement")]
        public float shootForce = 0.1f;
        public float moveSpeed = 5f;
        public float inputSensitivity = 1f;

        [Range(0f, 1f)]
        public float chanceForTwo = 0.75f;

        [Header("TileCube Visuals")] 
        public TileColor[] tileColors;
    }
}
