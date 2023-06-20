using System.Numerics;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace WoonyExcelCore
{
    public static class WoonyExcelCustomTypeSystem
    {
        public static bool ReadCustomType<T>(Type type, Dictionary<string, int> cellInfo, IRow row, ref T newItem, FieldInfo field) where T : new()
        {
            if (type == typeof(SceneField))
            {
                return SetSceneField(cellInfo, row, newItem, field);
            }
            else if (type == typeof(List<int>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(int));
            }
            else if (type == typeof(List<float>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(float));
            }
            else if (type == typeof(List<double>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(double));
            }
            else if (type == typeof(List<long>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(long));
            }
            else if (type == typeof(List<string>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(string));
            }
            else if (type == typeof(List<PassiveType>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(PassiveType));
            }
            else if (type == typeof(List<GoodsType>))
            {
                return SetValueList(cellInfo, row, newItem, field, typeof(GoodsType));
            }

            return false;
        }

        static bool SetValueList<T>(Dictionary<string, int> cellInfo, IRow row, T newItem, FieldInfo field, Type cellDataType) where T : new()
        {
            Dictionary<int, object> tmpDataMap = new Dictionary<int, object>();
            Dictionary<string, int> removedCellInfo = new Dictionary<string, int>();
            string key = "";
            while (IsRepeatable(cellInfo, field))
            {
                // cellinfo들에 일치되는 항목 탐색
                key = "";
                foreach (var item in cellInfo)
                {
                    if (item.Key.StartsWith(field.Name.ToLower()))
                    {
                        key = item.Key;
                        break;
                    }
                }
                // 항목의 뒷자리 숫자 가져옴
                int startPos = field.Name.Length;
                int index = int.Parse(key.Substring(startPos, key.Length - startPos));

                // cellNUm으로 값을 가져옴
                if (cellDataType == typeof(int))
                    tmpDataMap[index] = (int)row.GetCell(cellInfo[key]).NumericCellValue;
                else if (cellDataType == typeof(int))
                    tmpDataMap[index] = (BigInteger)row.GetCell(cellInfo[key]).NumericCellValue;
                else if (cellDataType == typeof(float))
                    tmpDataMap[index] = (float)row.GetCell(cellInfo[key]).NumericCellValue;
                else if (cellDataType == typeof(double))
                    tmpDataMap[index] = (double)row.GetCell(cellInfo[key]).NumericCellValue;
                else if (cellDataType == typeof(long))
                    tmpDataMap[index] = (long)row.GetCell(cellInfo[key]).NumericCellValue;
                else if (cellDataType == typeof(string))
                    tmpDataMap[index] = row.GetCell(cellInfo[key]).ToString();
                else if (cellDataType == typeof(PassiveType))
                    tmpDataMap[index] = Enum.Parse(cellDataType, row.GetCell(cellInfo[key]).StringCellValue, true);
                else if (cellDataType == typeof(GoodsType))
                    tmpDataMap[index] = Enum.Parse(cellDataType, row.GetCell(cellInfo[key]).StringCellValue, true);

                // 복원용 데이터 저장
                removedCellInfo[key] = cellInfo[key];
                // cellinfo에서 항목 제거
                cellInfo.Remove(key);
            }

            // 삭제한 데이터 백업
            foreach (var item in removedCellInfo)
                cellInfo[item.Key] = item.Value;

            // 가져온 맵을 리스트로 바꿔서 넣기
            // 동적 리스트 생성
            var tmpList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(cellDataType));
            for (int i = 0; i < tmpDataMap.Count; i++)
            {
                tmpList.Add(tmpDataMap[i]);

                field.SetValue(newItem, tmpList);
            }

            return true;

            static bool IsRepeatable(Dictionary<string, int> _cellInfo, FieldInfo field)
            {
                foreach (var item in _cellInfo)
                {
                    if (item.Key.StartsWith(field.Name.ToLower()))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        static bool SetSceneField<T>(Dictionary<string, int> cellInfo, IRow row, T newItem, FieldInfo field) where T : new()
        {
            foreach (var sceneGUID in AssetDatabase.FindAssets("t:scene", new[] { "Assets/_Scenes" }))
            {
                int cellIndex = cellInfo[field.Name.ToLower()];
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
                if (scenePath.Contains(row.GetCell(cellIndex).StringCellValue))
                {
                    UnityEngine.Object scene = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));
                    var assetsIndex = scenePath.IndexOf("Assets", StringComparison.Ordinal) + 7;
                    var extensionIndex = scenePath.LastIndexOf(".unity", StringComparison.Ordinal);
                    var sceneName = scenePath.Substring(assetsIndex, extensionIndex - assetsIndex);
                    field.SetValue(newItem, new SceneField(scene, sceneName));
                    break;
                }
            }

            return true;
        }
    }
}
