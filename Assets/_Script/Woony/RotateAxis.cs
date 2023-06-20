using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    Vector3 eulerAngles;
    [SerializeField] float speed = 25;
    [SerializeField] bool AxisX = false;
    [SerializeField] bool AxisY = false;
    [SerializeField] bool AxisZ = false;
    void Update()
    {
        eulerAngles = transform.localRotation.eulerAngles;

        if (AxisX)
            eulerAngles.x += speed * Time.unscaledDeltaTime;
        if (AxisY)
            eulerAngles.y += speed * Time.unscaledDeltaTime;
        if (AxisZ)
            eulerAngles.z += speed * Time.unscaledDeltaTime;

        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    public void UpdateSpeed(float speed)
    {
        this.speed = speed;
    }
}
