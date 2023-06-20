using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeviceVibrateSystem
{
    public static bool IsVibrate;

    public static void PlayerTakeHitVibrate()
    {
        if (!IsVibrate) return;

        Vibration.Vibrate(
            milliseconds: 100,
            amplitude: 64);
    }
}
