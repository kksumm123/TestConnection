using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPointerDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }

    float endTime;
    [SerializeField] float onClickDelay = 0.02f;
    void FixedUpdate()
    {
        if (buttonDown && endTime < Time.time)
        {
            endTime = Time.time + onClickDelay;
            button.onClick?.Invoke();
        }
    }

    bool buttonDown = false;
    [SerializeField] float requiredDownTime = 1f;
    Coroutine handle;
    public void OnPointerDown(PointerEventData eventData) => handle = StartCoroutine(DownCo());
    public void OnPointerUp(PointerEventData eventData)
    {
        if (handle != null)
            StopCoroutine(handle);

        buttonDown = false;
    }
    IEnumerator DownCo()
    {
        yield return new WaitForSeconds(requiredDownTime);
        buttonDown = true;
    }
}
