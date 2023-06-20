using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ErrorSystem = WoonyExcelCore.WoonyExcelErrorSystem;
using CustomTypeSystem = WoonyExcelCore.WoonyExcelCustomTypeSystem;

namespace WoonyExcelCore
{
    public class WoonyExcelReadSystem
    {
        static readonly bool READ_CONTINUE = true;
        static readonly bool READ_SUCESS = true;
        static readonly bool READ_FAIL = false;

        public static bool Read<T>(string sheetName, ISheet sheet, ref ICell currentCell, int index, Dictionary<string, int> cellInfo, List<T> result) where T : new()
        {
            IRow row = sheet.GetRow(index);
            // 행이 비었으면 continue
            if (row == null || row.Cells.All(x => x.CellType == CellType.Blank))
            {
                return READ_CONTINUE;
            }

            T newItem = new T();
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                Type type = field.FieldType;

                // 커스텀 타입이라면
                if (CustomTypeSystem.ReadCustomType(type, cellInfo, row, ref newItem, field))
                {
                    continue;
                }

                if (cellInfo.TryGetValue(field.Name.ToLower(), out int cellNum) == false)
                    continue;
                else if (row.GetCell(cellNum) == null)
                {
                    ErrorSystem.EmptyCell(sheetName: sheetName,
                                          row: index + 1,
                                          col: cellNum + 1,
                                          type: type);
                    return READ_FAIL;
                }

                currentCell = row.GetCell(cellNum);

                if (type == typeof(int))
                    field.SetValue(newItem, (int)row.GetCell(cellNum).NumericCellValue);
                else if (type == typeof(long))
                    field.SetValue(newItem, (long)row.GetCell(cellNum).NumericCellValue);
                else if (type == typeof(float))
                    field.SetValue(newItem, (float)row.GetCell(cellNum).NumericCellValue);
                else if (type == typeof(double))
                    field.SetValue(newItem, (double)row.GetCell(cellNum).NumericCellValue);
                else if (type == typeof(bool))
                    field.SetValue(newItem, row.GetCell(cellNum).BooleanCellValue);
                else if (type == typeof(string))
                    field.SetValue(newItem, row.GetCell(cellNum).ToString()); //row.GetCell(cellNum).StringCellValue
                else if (type.IsEnum)
                {
                    // Enum.Parse = 입력된 type에 대한 변수명 탐색
                    // (type, 변수명, 대소문자 무시
                    field.SetValue(newItem, Enum.Parse(type, row.GetCell(cellNum).StringCellValue, true));
                }

                currentCell = null;
            }

            result.Add(newItem);
            return READ_SUCESS;
        }
    }
}
