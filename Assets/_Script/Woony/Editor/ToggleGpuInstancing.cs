using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class ToggleGpuInstancing
{
    [MenuItem("WoonyEditor/Enable GPU Instancing")]
    static void EnableGpuInstancing()
    {
        SetGpuInstancing(true);
    }

    [MenuItem("WoonyEditor/Disable GPU Instancing")]
    static void DisableGpuInstancing()
    {
        SetGpuInstancing(false);
    }

    static void SetGpuInstancing(bool value)
    {
        foreach (var guid in AssetDatabase.FindAssets("t:Material", new[] { "Assets/_Materials" }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
            {
                material.enableInstancing = value;
                EditorUtility.SetDirty(material);
            }
        }
    }
}
