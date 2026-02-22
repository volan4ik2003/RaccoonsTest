using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }
}
