using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NPOI;
using System;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Linq;
using System.Reflection;
using ErrorSystem = WoonyExcelCore.WoonyExcelErrorSystem;
using ReadSystem = WoonyExcelCore.WoonyExcelReadSystem;
public class WoonyExcelSystem
{
    public static int START_ROW_INDEX = 2;
    static ISheet sheet;
    static ICell currentCell;

    public static List<T> ReadExcel<T>(string excelFilePath, string sheetName) where T : new()
    {
        Debug.Log($"엑셀파일 읽기 시작 {sheetName}, {excelFilePath}");

        List<T> result = new List<T>();
        sheet = null;
        currentCell = null;
        try
        {
            using var stream = new FileStream(excelFilePath, FileMode.Open);
            stream.Position = 0;
            // XSSFWorkbook = 엑셀파일
            XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
            sheet = xssWorkbook.GetSheet(sheetName);

            if (sheet == null)
            {
                ErrorSystem.NonExistSheet(sheetName, excelFilePath);
                return null;
            }

            // 헤더맵 만들기
            IRow headerRow = sheet.GetRow(0);
            Dictionary<string, int> cellInfo = new Dictionary<string, int>();
            for (int i = 0; i < headerRow.Count(); i++)
            {
                if (headerRow.GetCell(i) == null)
                {
                    ErrorSystem.InvisibleValueInCell(sheetName, headerRow.Cells[i].Address.ToString());
                    continue;
                }

                if (headerRow.GetCell(i).StringCellValue == "")
                    continue;

                string name = headerRow.GetCell(i).StringCellValue;
                cellInfo[name.ToLower()] = i;
            }

            // 값 순회하며 불러오기
            for (int index = START_ROW_INDEX; index <= sheet.LastRowNum; index++)
            {
                if (ReadSystem.Read(sheetName, sheet, ref currentCell, index, cellInfo, result) == false)
                {
                    return null;
                }
            }
        }
        catch (Exception e)
        {
            ErrorSystem.ExceptionError(e, sheetName, currentCell);
            throw;
        }

        Debug.Log($"엑셀파일 읽기 완료 {typeof(T).Name}, {excelFilePath}");
        return result;
    }
}