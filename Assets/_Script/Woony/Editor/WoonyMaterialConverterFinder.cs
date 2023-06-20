using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WoonyMaterialConverterFinder : EditorWindow
{
    static GUIStyle buttonStyle;
    static List<MaterialConverter> materialConverters = new List<MaterialConverter>();
    static WoonyMaterialConverterFinder woonyMaterialConverterController;
    static string searchStr;
    Vector2 scrollPosition;

    [MenuItem("WoonyEditor/Open WoonyMaterialConverterFinder")]
    static void Init()
    {
        searchStr = "";
        materialConverters = WoonyMethods.GetAllObjectsOnlyInScene<MaterialConverter>();

        if (woonyMaterialConverterController == null)
        {
            woonyMaterialConverterController = CreateInstance<WoonyMaterialConverterFinder>();
        }

        woonyMaterialConverterController.Show();
    }

    private void OnGUI()
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;

        GUILayout.Label("나는야 매테리얼 탐색기");
        GUILayout.Label($"총 {materialConverters.Count}개 탐색완료.");

        if (GUILayout.Button("갱신하기"))
        {
            materialConverters.Clear();
            Init();
        }

        GUILayout.Space(10);

        if (IsExistConverters() == false)
        {
            GUILayout.Label("씬에 MaterialConverter가 존재하지 않습니다");
        }

        MakeListToButtons();
    }

    private static bool IsExistConverters()
    {
        return materialConverters.Count != 0;
    }

    private void MakeListToButtons()
    {
        searchStr = EditorGUILayout.TextField("이름으로 검색하기", searchStr);

        GUILayout.Space(10);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (var converter in materialConverters)
        {
            if ((searchStr != string.Empty || searchStr != "") && converter.name.ToUpper().Contains(searchStr.ToUpper()) == false)
                continue;

            if (GUILayout.Button(converter.name, buttonStyle))
            {
                Selection.activeObject = converter;
                EditorGUIUtility.PingObject(converter);
            }
        }
        GUILayout.EndScrollView();
    }
}

[CustomEditor(typeof(MaterialConverterManager)), CanEditMultipleObjects]
public class MaterialConverterCE : Editor
{
    public override void OnInspectorGUI()
    {
        var materialConverter = target as MaterialConverterManager;

        if (GUILayout.Button("원본 매테리얼로 세팅"))
        {
            materialConverter.Test_ConvertToOrigin();
        }
        if (GUILayout.Button("변환 매테리얼로 세팅"))
        {
            materialConverter.Test_ConvertToNewMaterials();
        }

        base.OnInspectorGUI();
    }
}