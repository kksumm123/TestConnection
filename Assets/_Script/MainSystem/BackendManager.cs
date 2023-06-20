using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        Test();
    }

    private async void Test()
    {
        await Task.Run(() =>
        {
            BackendLoginSystem.Instance.CustomLogin("user1", "1234");

            BackendGameData.Instance.GameDataGet();

            if (BackendGameData.Instance.userData == null)
            {
                BackendGameData.Instance.GameDataInsert();
            }

            BackendGameData.Instance.LevelUp();
            BackendGameData.Instance.GameDataUpdate();
        });
    }
}
