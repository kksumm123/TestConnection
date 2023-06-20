using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Renderer)), DisallowMultipleComponent]
public class MaterialConverter : MonoBehaviour
{
    new Renderer renderer;
    List<Material> originalMaterials = new List<Material>();
    [SerializeField] List<MaterialConvertInfo> convertMaterialInfos = new List<MaterialConvertInfo>();
    MaterialConvertInfo tempInfo;
    MaterialConvertTheme theme;
    List<Material> convertMaterials = new List<Material>();
    bool isInit = false;
    bool isMatchedMaterialCount = false;

    public void OnAwakeManager(MaterialConvertTheme theme)
    {
        Initialize(theme);

        ConvertToNewMaterials();
    }

    private void Initialize(MaterialConvertTheme theme)
    {
        if (isInit) return;
        isInit = true;

        this.theme = theme;

        LinkComponent();
        SetConvertMaterials(theme);
        GetOriginMaterials();
    }

    private void LinkComponent()
    {
        renderer = GetComponent<Renderer>();
        WoonyMethods.Asserts(this, (renderer, nameof(renderer)));
    }

    private void SetConvertMaterials(MaterialConvertTheme theme)
    {
        convertMaterials = GetCurrentThemeMaterials(theme);
        IsValidConvertMaterials();
    }

    private List<Material> GetCurrentThemeMaterials(MaterialConvertTheme theme)
    {
        if (convertMaterialInfos == null || convertMaterialInfos.Count == 0)
        {
            Debug.LogError($"{transform.GetPath()} : convertMaterialInfos 설정이 필요합니다");
        }

        tempInfo = convertMaterialInfos.Where(x => x.theme == theme)
                                       .FirstOrDefault();

        if (tempInfo == null
            || (theme != MaterialConvertTheme.None
                && tempInfo.theme == MaterialConvertTheme.None))
        {
            Debug.LogError($"{theme}에 일치하는 정보가 등록되지 않았습니다\n{transform.GetPath()}", transform);
            return new List<Material>();
        }

        return tempInfo.convertMaterials;
    }

    private void IsValidConvertMaterials()
    {
        if (convertMaterials == null || convertMaterials.Count == 0)
        {
            Debug.LogError($"{transform.GetPath()} : 테마에 일치하는 매테리얼을 받아오지 못했습니다");
        }
    }

    private void GetOriginMaterials()
    {
        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        {
            originalMaterials.Add(renderer.sharedMaterials[i]);
        }

        isMatchedMaterialCount = IsMatchedMaterialCount();
        WoonyMethods.Asserts(this, (isMatchedMaterialCount, "렌더러에 할당된 매테리얼 개수와 convertMaterials에 등록된 매테리얼 개수가 일치하지 않습니다."));
    }

    private bool IsMatchedMaterialCount()
    {
        return originalMaterials.Count == convertMaterials.Count;
    }

    void ConvertToNewMaterials()
    {
        if (renderer == null) return;
        if (isMatchedMaterialCount == false) return;
        if (convertMaterials == null || convertMaterials.Count == 0) return;

        renderer.materials = convertMaterials.ToArray();
#if UNITY_EDITOR
        EditorUtility.SetDirty(transform);
#endif
    }

    void ConvertToOrigin()
    {
        if (renderer == null) return;
        if (isMatchedMaterialCount == false) return;

        renderer.materials = originalMaterials.ToArray();
#if UNITY_EDITOR
        EditorUtility.SetDirty(transform);
#endif
    }

#if UNITY_EDITOR
    public void Test_ConvertToNewMaterials(MaterialConvertTheme theme)
    {
        if (isInit == false || this.theme != theme) Initialize(theme);

        SetConvertMaterials(theme);
        ConvertToNewMaterials();
    }

    public void Test_ConvertToOrigin()
    {
        if (isInit == false) Initialize(MaterialConvertTheme.None);

        ConvertToOrigin();
    }

    void OnValidate()
    {
        // ConvertMaterialInfos의 리스트가 추가될 때 원본 매테리얼을 설정해주기 위한 세팅
        if (renderer == null) renderer = GetComponent<Renderer>();

        foreach (var item in convertMaterialInfos)
        {
            if (item.convertMaterials == null || item.convertMaterials.Count == 0)
            {
                item.convertMaterials = renderer.sharedMaterials.ToList();
            }
        }
    }
#endif

    public enum MaterialConvertTheme
    {
        None,
        RedTheme,
        BlueTheme,
        PoisonTheme,
        ATheme,
        BTheme,
        CTheme,
    }

    [System.Serializable]
    public class MaterialConvertInfo
    {
        public MaterialConvertTheme theme;
        public List<Material> convertMaterials = new List<Material>();
    }
}
