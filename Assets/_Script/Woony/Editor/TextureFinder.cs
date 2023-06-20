using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TextureFinder : EditorWindow
{
    static GUIStyle buttonStyle;
    static List<string> texturePaths = new List<string>();
    static TextureFinder textureFinder;
    static string searchStr;

    [MenuItem("WoonyEditor/Open TextureFinder %#E", priority = 0)]
    static void Init()
    {
        searchStr = "";

        if (texturePaths.Count() == 0)
        {
            foreach (var textureUID in AssetDatabase.FindAssets("t:Texture", new[] { "Assets" }))
                texturePaths.Add(AssetDatabase.GUIDToAssetPath(textureUID));

            texturePaths.Sort();
        }
        if (textureFinder == null)
            textureFinder = CreateInstance<TextureFinder>();

        textureFinder.Show();
    }

    Vector2 scrollPosition;
    void OnGUI()
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;

        if (GUILayout.Button("목록 갱신하기"))
        {
            texturePaths.Clear();
            Init();
        }

        searchStr = EditorGUILayout.TextField("이름으로 검색하기", searchStr);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (var texturePath in texturePaths)
        {
            if ((searchStr != string.Empty || searchStr != "") && texturePath.ToUpper().Contains(searchStr.ToUpper()) == false)
                continue;

            if (GUILayout.Button(texturePath, buttonStyle))
            {
                UnityEngine.Object texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture));
                Selection.activeObject = texture;
                EditorGUIUtility.PingObject(texture);
                EditorWindow.GetWindow<TextureFinder>().Close();
            }
        }
        GUILayout.EndScrollView();
    }
}
