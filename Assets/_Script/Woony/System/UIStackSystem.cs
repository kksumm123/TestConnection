using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStackSystem
{
    public class UICloseInfo
    {
        public Transform ui;
        public Action close;
    }

    private List<UICloseInfo> _uiStack = new List<UICloseInfo>();

    private int IsExistInUIStack(Transform ui)
    {
        for (int i = 0; i < _uiStack.Count; i++)
        {
            if (_uiStack[i].ui == ui)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Show에서 Push 한 경우, Close에서 Remove 호출해야 함.
    /// </summary>
    public void Push(Transform ui, Action close)
    {
        Remove(ui);

        _uiStack.Add(new UICloseInfo()
        {
            ui = ui,
            close = close
        });
    }

    public void Remove(Transform ui)
    {
        int index = IsExistInUIStack(ui);
        if (index < 0) return;
        _uiStack.RemoveAt(index);
    }

    /// <summary>
    /// 성공적으로 UI를 닫았으면 true, 스택이 비어있으면 false
    /// </summary>
    public bool Pop()
    {
        bool isClosed = false;
        if (_uiStack.Count == 0)
        {
            return isClosed;
        }

        UICloseInfo uiCloseInfo = null;
        int index = _uiStack.Count - 1;
        while (index >= 0)
        {
            uiCloseInfo = _uiStack[index];
            if (uiCloseInfo.ui.gameObject.activeSelf)
            {
                uiCloseInfo.close?.Invoke();
                isClosed = true;
                break;
            }
            else
            {
                _uiStack.RemoveAt(index);
            }
            index--;
        }

        return isClosed;
    }

    public void ForceClear()
    {
        _uiStack.Clear();
    }
}
