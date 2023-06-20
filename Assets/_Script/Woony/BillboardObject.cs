using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BillboardObject : MonoBehaviour
{
    void LateUpdate() => transform.rotation = Camera.main.transform.rotation;
}
