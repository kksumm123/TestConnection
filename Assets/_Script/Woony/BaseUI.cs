using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected WoonyMethods.CustomEase showEase;
    protected WoonyMethods.CustomEase closeEase;
    protected bool isInit = false;

    public void Initialize()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        transform.SetAsLastSibling();

        //showEase = GameResourcesManager.Instance.GetUIShowEase();
        //closeEase = GameResourcesManager.Instance.GetUICloseEase();

        OnInitialize();
        isInit = true;
    }

    protected virtual void OnInitialize() { }

    protected virtual void ShowUI()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }
    protected virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }
}