using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOHelper : MonoBehaviour
{
    [SerializeField] bool onEnable;
    [SerializeField] bool onDisble;
    [SerializeField] bool onDestroy;
    private void OnEnable()
    {
        if (onEnable)
            Debug.Log($"{gameObject} 활성화 OnEanble", gameObject);
    }
    private void OnDisable()
    {
        if (onDisble)
            Debug.Log($"{gameObject} OnDisable", gameObject);
    }
    private void OnDestroy()
    {
        if (onDestroy)
            Debug.Log($"{gameObject} OnDestroy", gameObject);
    }
}
