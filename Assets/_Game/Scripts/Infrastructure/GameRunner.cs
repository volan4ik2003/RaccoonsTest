using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        private void Awake()
        {
            if (FindFirstObjectByType<GameBootstrapper>() != null) return;

            var currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == SceneNames.Boot) return;

            StartupSceneTracker.SceneToLoadAfterBoot = currentScene;
            SceneManager.LoadScene(SceneNames.Boot);
        }
    }
}