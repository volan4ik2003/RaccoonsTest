using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _Game.Scripts.Editor
{
    [InitializeOnLoad]
    public static class SceneSelectorToolbar
    {
        private static string[] _scenePaths;
        private static string[] _sceneNames;
        private static int _selectedIndex;

        static SceneSelectorToolbar()
        {
            _scenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            _sceneNames = _scenePaths
                .Select(path => System.IO.Path.GetFileNameWithoutExtension(path))
                .ToArray();

            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.update -= OnUpdate;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(10, 300, 100, 24), EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUILayout.Popup(_selectedIndex, _sceneNames, EditorStyles.toolbarPopup, GUILayout.Width(220));
            if (EditorGUI.EndChangeCheck())
            {
                _selectedIndex = newIndex;
                if (!string.IsNullOrEmpty(_scenePaths[_selectedIndex]))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(_scenePaths[_selectedIndex]);
                    }
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            Handles.EndGUI();
        }
    }
}