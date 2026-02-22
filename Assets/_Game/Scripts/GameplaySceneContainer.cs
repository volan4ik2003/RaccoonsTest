using UnityEngine;

namespace _Game.Scripts
{
    public class GameplaySceneContainer : MonoBehaviour
    {
        public static GameplaySceneContainer Instance { get; private set; }

        [field: SerializeField] public Transform UIRoot { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}