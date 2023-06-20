using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        var backend = Backend.Initialize(true);

        if (backend.IsSuccess())
        {
            Debug.Log($"성공: {backend}");
        }
        else
        {
            Debug.Log($"실패: {backend}");
        }
    }
}
