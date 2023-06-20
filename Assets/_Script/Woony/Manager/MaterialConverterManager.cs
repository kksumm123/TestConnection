using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialConverterManager : Singleton<MaterialConverterManager>
{
    [SerializeField] MaterialConverter.MaterialConvertTheme theme;

    void Awake()
    {
        if (theme == MaterialConverter.MaterialConvertTheme.None) return;

        GetAllMaterialConverters().ForEach(x => x.OnAwakeManager(theme));
    }

    private List<MaterialConverter> GetAllMaterialConverters()
    {
        return WoonyMethods.GetAllObjectsOnlyInScene<MaterialConverter>();
    }

#if UNITY_EDITOR
    public void Test_ConvertToNewMaterials()
    {
        GetAllMaterialConverters().ForEach(x => x.Test_ConvertToNewMaterials(theme));
    }

    public void Test_ConvertToOrigin()
    {
        GetAllMaterialConverters().ForEach(x => x.Test_ConvertToOrigin());
    }
#endif
}
