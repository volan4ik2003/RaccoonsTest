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
        public float shootForce = 15f;
        public float moveSpeed = 5f;
        public float inputSensitivity = 1f;

        [Tooltip("Right and left restrictions")]
        public float horizontalLimit = 2.5f;

        [Header("Spawning & Merging")]
        [Range(0f, 1f)]
        public float chanceForTwo = 0.75f;

        [Tooltip("Delay between spawning tiles")]
        public int spawnDelayMs = 500;

        [Tooltip("Jump force while merging")]
        public float mergeJumpForce = 4f;

        [Header("TileCube Visuals")]
        public TileColor[] tileColors;
    }
}