using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Infrastructure
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(gameObject);
        }
    
        public void LoadScene(string name, Action OnLoaded = null)
        {
            StartCoroutine(LoadSceneCoroutine(name, OnLoaded));
        }
    
        public IEnumerator LoadSceneCoroutine(string name, Action OnLoaded = null)
        {
            AsyncOperation waitNextScene =  SceneManager.LoadSceneAsync(name);

            while (!waitNextScene.isDone) 
                yield return null;
            
            OnLoaded?.Invoke();
        }
    }
}
