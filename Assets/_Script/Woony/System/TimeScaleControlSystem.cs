using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class TimeScaleControlSystem
{
#if UNITY_EDITOR
    private static EditorWindow _gameView;
#endif

    public static float TimeScale
    {
        get => Time.timeScale;
        set
        {
#if UNITY_EDITOR
            var text = $"TimeScale : {Time.timeScale} -> {value}";
            Debug.Log(text);

            foreach (SceneView item in SceneView.sceneViews)
                item.ShowNotification(new GUIContent(text));

            if (_gameView == null)
            {
                var gameViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
                _gameView = EditorWindow.GetWindow(gameViewType);
            }
            _gameView.ShowNotification(new GUIContent(text));
#endif

            Time.timeScale = value;
        }
    }
}
