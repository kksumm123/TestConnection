using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendLoginSystem : SystemSingleton<BackendLoginSystem>
{
    public void CustomSignUp(string id, string pw)
    {
        var bro = BackEnd.Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log($"회원 가입 성공 : {bro}");
        }
        else
        {
            Debug.Log($"회원 가입 실패 : {bro}");
        }
    }

    public void CustomLogin(string id, string pw)
    {
        var bro = BackEnd.Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log($"로그인 성공 : {bro}");
        }
        else
        {
            Debug.Log($"로그인 실패 : {bro}");
        }
    }

    public void UpdateNickname(string nickname)
    {
        var bro = BackEnd.Backend.BMember.UpdateNickname(nickname);


        if (bro.IsSuccess())
        {
            Debug.Log($"닉네임 변경 성공 : {bro}");
        }
        else
        {
            Debug.Log($"닉네임 변경 실패 : {bro}");
        }
    }
}
