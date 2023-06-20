using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WoonySceneSelector : EditorWindow
{
    static GUIStyle buttonStyle;
    static List<string> scenePaths = new List<string>();
    static WoonySceneSelector woonySceneSelectorWindow;
    static string searchStr;

    [MenuItem("WoonyEditor/Open WoonySceneSelector %#W", priority = 0)]
    static void Init()
    {
        searchStr = "";

        if (scenePaths.Count() == 0)
        {
            foreach (var sceneGUID in AssetDatabase.FindAssets("t:scene", new[] { "Assets/_Scenes" }))
                scenePaths.Add(AssetDatabase.GUIDToAssetPath(sceneGUID));

            scenePaths.Sort();
        }
        if (woonySceneSelectorWindow == null)
            woonySceneSelectorWindow = CreateInstance<WoonySceneSelector>();

        woonySceneSelectorWindow.Show();
    }

    Vector2 scrollPosition;
    void OnGUI()
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;

        if (GUILayout.Button("씬 목록 갱신하기"))
        {
            scenePaths.Clear();
            Init();
        }

        searchStr = EditorGUILayout.TextField("이름으로 검색하기", searchStr);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (var scenePath in scenePaths)
        {
            if ((searchStr != string.Empty || searchStr != "") && scenePath.ToUpper().Contains(searchStr.ToUpper()) == false)
                continue;

            if (GUILayout.Button(scenePath, buttonStyle))
            {
                UnityEngine.Object scene = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));
                Selection.activeObject = scene;
                EditorGUIUtility.PingObject(scene);

                if (SceneManager.GetActiveScene().isDirty)
                {
                    if (EditorUtility.DisplayDialog("씬 불러오기", "현재 씬에서 저장되지 않은 내역이 있습니다.\n씬을 불러오겠습니까?", "불러와~", "안불러와~"))
                        OpenScene(scenePath);
                }
                else
                    OpenScene(scenePath);
            }
        }
        GUILayout.EndScrollView();
    }

    private static void OpenScene(string scenePath)
    {
        EditorSceneManager.OpenScene(scenePath);
        EditorWindow.GetWindow<WoonySceneSelector>().Close();
    }
}