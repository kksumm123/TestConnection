using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WoonyModifiedMaterialObjectFinder : EditorWindow
{
    static GUIStyle buttonStyle;
    static List<Renderer> renderers = new List<Renderer>();
    static List<Renderer> result = new List<Renderer>();
    static PropertyModification[] modifications;
    static Transform targetObject;
    static WoonyModifiedMaterialObjectFinder WoonyModifiedMaterialObjectFinderw;
    static bool isNotAttachedMaterialConverter = false;
    static string searchStr;
    Vector2 scrollPosition;

    [MenuItem("WoonyEditor/Open WoonyModifiedMaterialObjectFinder")]
    static void Init()
    {
        searchStr = "";
        renderers = WoonyMethods.GetAllObjectsOnlyInScene<Renderer>();
        renderers = renderers.Where(x => (x is ParticleSystemRenderer) == false
                                         && x.sharedMaterial.name.ToLower().Contains("lilitaone") == false).ToList();

        result.Clear();
        GetModifiedRenderer();

        if (WoonyModifiedMaterialObjectFinderw == null)
        {
            WoonyModifiedMaterialObjectFinderw = CreateInstance<WoonyModifiedMaterialObjectFinder>();
        }

        WoonyModifiedMaterialObjectFinderw.Show();
    }

    private static void GetModifiedRenderer()
    {
        foreach (var renderer in renderers)
        {
            modifications = PrefabUtility.GetPropertyModifications(renderer);
            if (modifications == null) continue;

            foreach (var modification in modifications)
            {
                if (IsModifiedMaterial(modification))
                {
                    if (modification.target == null)
                    {
                        Debug.Log($"{renderer.name}, {modification}", renderer);
                        continue;
                    }

                    if (IsAbleToAdd(renderer, modification))
                    {
                        result.Add(renderer);
                        break;
                    }
                }
            }
        }

        static bool IsModifiedMaterial(PropertyModification modification)
        {
            return modification.propertyPath.ToLower().Contains("material");
        }

        static bool IsAbleToAdd(Renderer renderer, PropertyModification modification)
        {
            return renderer.name == modification.target.name
                   && renderer.sharedMaterial == (modification.objectReference as Material)
                   && result.Contains(renderer) == false;
        }
    }


    private void OnGUI()
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;

        GUILayout.Label("나는야 수정된 렌더러 탐색기");
        GUILayout.Label($"총 {result.Count}개 탐색완료.");

        if (GUILayout.Button("갱신하기"))
        {
            result.Clear();
            Init();
        }

        GUILayout.Space(10);

        ExceptIfExistConverter();

        if (IsExistRenderers() == false)
        {
            GUILayout.Label("씬에 수정된 렌더러가 존재하지 않습니다");
        }

        MakeListToButtons();
    }

    private static void ExceptIfExistConverter()
    {
        isNotAttachedMaterialConverter = GUILayout.Toggle(isNotAttachedMaterialConverter, "매테리얼 컨버터가 붙어있으면 제외");
        if (isNotAttachedMaterialConverter)
        {
            result = result.Where(x => x.GetComponent<MaterialConverter>() == null).ToList();
        }
    }

    private static bool IsExistRenderers()
    {
        return result.Count != 0;
    }

    private void MakeListToButtons()
    {
        searchStr = EditorGUILayout.TextField("이름으로 검색하기", searchStr);

        GUILayout.Space(10);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (var converter in result)
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
