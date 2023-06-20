using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class SimpleInt
{
    static int tempIndex;
    static BigInteger quotient;
    // 소수점 n번째 자릿수까지 표시할 지.
    static int numberOfDecimalPlaces = 1;
    static int orderValue = 1000;
    static BigInteger tempResult;
    static int tempDecimalPlaces;
    static List<string> simpleIntStrings = new List<string>() { "", "K", "M", "B", "a", "b", "c", "d", "e", "f" };

    static string GetSimpleInt(BigInteger value, int tempIndex)
    {
        // 빅인트 수치 계산
        tempResult = GetBigIntegerResult(tempIndex); 

        // 소수점 계산
        tempDecimalPlaces = (int)Math.Pow(10, numberOfDecimalPlaces);
        tempResult = BigInteger.Divide(value * tempDecimalPlaces, tempResult);
        float result = (float)tempResult / tempDecimalPlaces;
        return $"{result}{simpleIntStrings[tempIndex]}";

        static BigInteger GetBigIntegerResult(int tempIndex)
        {
            return BigInteger.Pow(orderValue, tempIndex);
        }
    }

    public static string ToSimpleInt(BigInteger value)
    {
        if (value == 0)
        {
            return value.ToString();
        }

        return GetSimpleInt(value, GetSimpleIntStringIndex(value));

        static int GetSimpleIntStringIndex(BigInteger value)
        {
            for (tempIndex = 0; tempIndex < simpleIntStrings.Count; tempIndex++)
            {
                quotient = BigInteger.Divide(value, BigInteger.Pow(orderValue, tempIndex));
                if (quotient == 0)
                {
                    break;
                }
            }

            return tempIndex - 1;
        }
    }
}
