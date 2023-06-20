using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WoonyEditor : Editor
{
#if UNITY_EDITOR
    [MenuItem("WoonyEditor/SaveGame/Initialize", priority = 0)]
    public static void Initialize()
    {
        SaveGame.DeleteAll();
        PlayerPrefs.DeleteAll();
        Debug.Log("게임 세이브데이터 초기화 완료");
        //EditorUtility.DisplayDialog("초기화 완료", "게임 세이브데이터 초기화 완료", "닫기");
    }

    [MenuItem("WoonyEditor/SaveGame/Open SaveFile Folder", priority = 0)]
    public static void OpenPathSaveFile()
    {
        var path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\..\LocalLow\Aloha Factory\{Application.productName}";
        System.Diagnostics.Process.Start(path);
    }
#endif
}
