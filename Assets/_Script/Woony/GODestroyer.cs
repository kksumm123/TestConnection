using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GODestroyer : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
