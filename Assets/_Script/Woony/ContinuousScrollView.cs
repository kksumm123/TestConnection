using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ContinuousScrollView : MonoBehaviour
{
    enum ScrollDirectionType
    {
        Horizontal,
        Vertical,
    }

    float contentLength => GetValue(onHorizontal: content.anchoredPosition.x,
                                    onVertival: content.anchoredPosition.y);

    [SerializeField] ScrollDirectionType scrollDirectionType = ScrollDirectionType.Horizontal;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform content;
    Dictionary<int, RectTransform> childsMap = new Dictionary<int, RectTransform>();
    RectTransform tempChild;
    Vector2 tempVec2;

    float spacing;
    float childLenght;
    float orderLenght;

    [SerializeField] bool useAutoScroll = false;
    [SerializeField] float autoScrollSpeed = 150;

    private void Start()
    {
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

        InitChildsMap();
        InitVariables();

        if (useAutoScroll)
        {
            StartCoroutine(AutoScrollCo());
        }
    }

    void InitChildsMap()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            childsMap[i] = content.GetChild(i).GetComponent<RectTransform>();
        }
    }

    void InitVariables()
    {
        spacing = (float)GetValue(onHorizontal: content.GetComponent<HorizontalLayoutGroup>()?.spacing,
                                  onVertival: content.GetComponent<VerticalLayoutGroup>()?.spacing);
        childLenght = GetValue(onHorizontal: childsMap[0].sizeDelta.x,
                               onVertival: childsMap[0].sizeDelta.y);
        orderLenght = childLenght + spacing;
        scrollRect.onValueChanged.AddListener((x) => UpdateContent(x));
    }

    void UpdateContent(Vector2 vector2)
    {
        InvokeAction(onHorizontal: UpdateInHorizontal,
                     onVertical: UpdateInVertical);

        void UpdateInHorizontal()
        {
            if (contentLength < -orderLenght)
            {
                MoveChildFirstToLast();
            }
            else if (contentLength > 0)
            {
                MoveChildLastToFirst();
            }
        }

        void UpdateInVertical()
        {
            if (contentLength > orderLenght)
            {
                MoveChildFirstToLast();
            }
            else if (contentLength < 0)
            {
                MoveChildLastToFirst();
            }
        }
    }

    void MoveChildFirstToLast()
    {
        tempChild = childsMap[0];
        for (int i = 0; i < childsMap.Count - 1; i++)
        {
            childsMap[i] = childsMap[i + 1];
        }
        childsMap[childsMap.Count - 1] = tempChild;
        tempChild.SetAsLastSibling();

        ChangeContentAnchoredPosition(orderLenght);
    }

    void MoveChildLastToFirst()
    {
        tempChild = childsMap[childsMap.Count - 1];
        for (int i = childsMap.Count - 2; i >= 0; i--)
        {
            childsMap[i + 1] = childsMap[i];
        }
        childsMap[0] = tempChild;
        tempChild.SetAsFirstSibling();

        ChangeContentAnchoredPosition(-orderLenght);
    }

    void ChangeContentAnchoredPosition(float value)
    {
        tempVec2 = content.anchoredPosition;
        InvokeAction(onHorizontal: () => tempVec2.x += value,
                     onVertical: () => tempVec2.y -= value);
        content.anchoredPosition = tempVec2;
    }

    IEnumerator AutoScrollCo()
    {
        var tempValue = new Vector2(0, 0);
        var isTrue = true;
        while (isTrue)
        {
            InvokeAction(onHorizontal: () => tempValue.x = autoScrollSpeed * Time.deltaTime,
                         onVertical: () => tempValue.y = -autoScrollSpeed * Time.deltaTime);

            content.anchoredPosition += tempValue;
            yield return null;
        }
    }

    void InvokeAction(Action onHorizontal, Action onVertical)
    {
        switch (scrollDirectionType)
        {
            case ScrollDirectionType.Horizontal:
                onHorizontal?.Invoke();
                break;
            case ScrollDirectionType.Vertical:
                onVertical?.Invoke();
                break;
        }
    }

    T GetValue<T>(T onHorizontal, T onVertival)
    {
        switch (scrollDirectionType)
        {
            case ScrollDirectionType.Horizontal:
                return onHorizontal;
            case ScrollDirectionType.Vertical:
                return onVertival;
            default:
                return default(T);
        }
    }
}