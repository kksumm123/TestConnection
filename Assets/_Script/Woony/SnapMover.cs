using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapMover : MonoBehaviour
{
    [SerializeField] float snapUnitValue = 1;
    void Start()
    {
        // 게임이 플레이중이면 해당 컴포넌트 부수기
        if (Application.isPlaying)
            Destroy(this);
    }

    void Update()
    {
        transform.position = transform.position.ToVector3Snap(snapUnitValue);
    }
}
public static class SnapClass
{
    static public Vector3 ToVector3Snap(this Vector3 v3, float snapUnitValue)
    {
        return new Vector3(v3.x - (v3.x % snapUnitValue),
                           v3.y,
                           v3.z - (v3.z % snapUnitValue));
    }
}