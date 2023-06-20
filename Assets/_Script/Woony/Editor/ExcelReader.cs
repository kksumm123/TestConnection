using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;

public class ExcelReader : EditorWindow
{
    private const string KeyExcel = "Excel";
    private const string KeyTextTable = "TextTable";
    private static string excelFilePath;
    private static string textTableFilePath;
    private static ExcelReader excelReader;
    private static Action readAllExcel;

    //private static SceneInfoSO _sceneInfoSO;
    //private static StageInfoSO _stageInfoSO; 

    [MenuItem("WoonyEditor/ExcelReader", priority = 0)]
    private static void Init()
    {
        excelReader = GetWindow(typeof(ExcelReader)) as ExcelReader;

        if (excelFilePath.IsEmpty())
            excelFilePath = EditorPrefs.GetString(KeyExcel, "");
        if (textTableFilePath.IsEmpty())
            textTableFilePath = EditorPrefs.GetString(KeyTextTable, "");

        // 추가 필요
        //InitSO(ref _sceneInfoSO);
        //InitSO(ref _stageInfoSO); 
        excelReader.Show();
    }

    private void OnGUI()
    {
        if (excelReader == null) Init();
        SelectExcelFrame(ref excelFilePath, KeyExcel);
        SelectExcelFrame(ref textTableFilePath, KeyTextTable);

        readAllExcel = null;
        // 추가 필요
        //CreateReadExcelButton<SceneInfoSO, SceneInfo>(ref _sceneInfoSO, excelFilePath);
        //CreateReadExcelButton<StageInfoSO, StageInfo>(ref _stageInfoSO, excelFilePath); 

        GUILayout.Space(10);
        GUILayout.Label("텍스트 테이블");
        //CreateReadExcelButton<TextInfoSO, TextInfo>(ref _textInfoSO, textTableFilePath);

        if (GUILayout.Button("전체 읽어오기"))
        {
            if (IsSelectedExcelFiles())
            {
                WoonyMethods.AlertError("엑셀 파일을 선택해주세요.");
                return;
            }

            readAllExcel();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }
    }

    private void ReadInfo<T>(BaseDataTableSO infoSO, string filePath) where T : new()
    {
        try
        {
            infoSO.ReadInfo(JsonConvert.SerializeObject(WoonyExcelSystem.ReadExcel<T>(filePath, infoSO.sheetName)));
        }
        catch (Exception e)
        {
            WoonyMethods.AlertError($"{typeof(T)} read fail, 콘솔 로그를 확인해주세요.");
            Debug.LogError(e);
            return;
        }
    }

    private void SelectExcelFrame(ref string filePath, string Key)
    {
        GUILayout.Label($"선택된 엑셀 파일 : \n{filePath}");
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("엑셀파일 선택"))
            {
                SelectExcelFile(ref filePath, Key);
                CloseAndInit();
                Debug.Log($"엑셀파일 재선택 완료 {filePath}");
            }
            if (filePath.IsEmpty() == false)
            {
                if (GUILayout.Button("엑셀파일 열기"))
                {
                    System.Diagnostics.Process.Start(filePath);
                }
            }
        }
        GUILayout.EndHorizontal();

        void SelectExcelFile(ref string filePath, string Key)
        {
            filePath = EditorUtility.OpenFilePanel("엑셀 파일 선택", "Assets/_Excel", "xlsx");
            if (filePath.IsEmpty()) return;
            EditorPrefs.SetString(Key, filePath);
        }
    }

    private static bool IsSelectedExcelFiles()
    {
        return excelFilePath.IsEmpty()
            || textTableFilePath.IsEmpty();
    }

    // 2023-05-09 Woony
    // ref 타입으로 so를 넘겨주지 않으면 외부 static 변수에 값 할당이 되지않아 editortool이 제대로 동작하지 않음.
    private static void InitSO<T>(ref T t) where T : ScriptableObject
    {
        if (t != null) return;
        var soPath = EditorPrefs.GetString(typeof(T).ToString(), string.Empty);
        if (soPath.IsEmpty()) return;
        t = (T)AssetDatabase.LoadAssetAtPath(soPath, typeof(T));
    }

    // 2023-05-09 Woony
    // ref 타입으로 so를 넘겨주지 않으면 외부 static 변수에 값 할당이 되지않아 editortool이 제대로 동작하지 않음.
    private void CreateReadExcelButton<SO, ReadType>(ref SO so, string excelPath)
        where SO : BaseDataTableSO
        where ReadType : new()
    {
        Type soType = typeof(SO);
        var soVar = so as BaseDataTableSO;
        Action readExcel = () => ReadInfo<ReadType>(soVar, excelPath);
        readAllExcel += readExcel;
        readAllExcel += () => EditorUtility.SetDirty(soVar);

        GUILayout.BeginHorizontal();
        {
            so = EditorGUILayout.ObjectField(so, soType, false) as SO;

            if (so != null)
            {
                if (GUILayout.Button($"SO 경로 저장"))
                {
                    EditorPrefs.SetString(soType.ToString(), AssetDatabase.GetAssetPath(so));
                }

                if (GUILayout.Button($"{so.name} 읽어오기"))
                {
                    if (IsSelectedExcelFiles())
                    {
                        WoonyMethods.AlertError("엑셀 파일 또는 텍스트 테이블을 선택해주세요.");
                        return;
                    }

                    readExcel?.Invoke();
                    EditorUtility.SetDirty(so);
                    EditorApplication.ExecuteMenuItem("File/Save Project");
                }
            }
            else
            {
                if (GUILayout.Button($"{soType.Name} SO 불러오기"))
                {
                    var soPath = EditorPrefs.GetString(soType.ToString(), string.Empty);
                    if (soPath.IsEmpty())
                        WoonyMethods.AlertError($"{soType} 타입에 대한 저장된 경로가 없습니다");
                    else
                    {
                        so = (SO)AssetDatabase.LoadAssetAtPath(soPath, soType);
                        EditorApplication.ExecuteMenuItem("File/Save Project");
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    private static void CloseAndInit()
    {
        GetWindow<ExcelReader>()?.Close();
        Init();
    }
}
