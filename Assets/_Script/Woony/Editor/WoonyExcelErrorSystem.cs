using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoonyExcelCore
{
    public static class WoonyExcelErrorSystem
    {
        static void AlertError(string text)
        {
            WoonyMethods.AlertError(text);
        }

        public static void NonExistSheet(string sheetName, string excelFilePath)
        {
            AlertError($"{sheetName}, 엑셀 파일에 존재하지 않는 시트명\n파일 : {excelFilePath}");
        }

        public static void InvisibleValueInCell(string sheetName, string cellAddress)
        {
            AlertError($"sheetName : '{sheetName}', cell : '{cellAddress}' 해당 셀에 보이지 않는 값이 있습니다.\n" +
                       "해당 빈값은 제외하고 엑셀읽기를 실행하오나, 의도치 않은 테이블값이 입력될 수도 있습니다.\n" +
                       "오류방지를 위해 해당 셀을 꼭 수정해주세요");
        }

        public static void EmptyCell(string sheetName, int row, int col, Type type)
        {
            AlertError("현재 행 중 빈칸이 존재합니다\n" +
                       $"시트명 = {sheetName}\n" +
                       $"{row}행 {col}열, type = {type}");
        }

        public static void ExceptionError(Exception e, string sheetName, ICell currentCell)
        {

            if (e.ToString().Contains("Sharing violation on path"))
            {
                AlertError("엑셀 파일이 열린 상태에선 읽어올 수 없습니다");
            }
            else if (e.ToString().Contains("Path is empty"))
            {
                AlertError("엑셀 파일이 선택되지 않았습니다");
            }
            else if (e.ToString().Contains("Could not find file"))
            {
                AlertError("경로에 해당 파일명의 엑셀이 존재하지 않습니다.");
            }
            else if (e.ToString().Contains("KeyNotFoundException"))
            {
                AlertError($"{NoticeCurrentCell()}엑셀내 변수명과 일치되는 내용이 없습니다\n엑셀파일 내용과 변수명, 대소문자 등 확인해주세요\n또는 우니를 불러주세요");
            }
            else if (e.ToString().Contains("InvalidOperationException"))
            {
                AlertError($"{NoticeCurrentCell()}\n클래스 변수의 type과 엑셀 데이터의 셀 type이 일치하지 않습니다.\nex)\n변수type = int, cell = text cell\n변수type = string, cell = numeric cell.");
            }
            else if (e.ToString().Contains("was not found") && e.ToString().Contains("Enum"))
            {
                AlertError($"{NoticeCurrentCell()}현재 enum에 대한 일치하는 enum에 대한 내용이 없습니다");
            }
            else if (e.ToString().Contains("NullReferenceException"))
            {
                AlertError($"{NoticeCurrentCell()}아마도 시트가 존재하지 않거나\n시트명과 클래스명이 일치하지 않을 수도 있습니다.\n확인해주세요\n만약 위 상황이 아니라면 우니를 불러주세요");
            }

            string NoticeCurrentCell()
            {
                return $"sheetName : {sheetName}, {(currentCell != null ? "셀 주소 " + currentCell.Address.ToString() : string.Empty)}\n";
            }
        }
    }
}
