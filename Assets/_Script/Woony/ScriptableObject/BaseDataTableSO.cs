using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDataTableSO : ScriptableObject
{
    public abstract string sheetName { get; }

    public T Get<T>() where T : class
    {
        return this is T ? this as T : null;
    }

    public abstract void ReadInfo(string jsonString);
}
