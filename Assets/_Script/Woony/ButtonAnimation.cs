using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] Transform target;

    private Vector3 localScale;
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        GetComponent<Button>().onClick.AddListener(StartPunchAnimation);

        if (target == null)
            target = transform;

        localScale = target.localScale;
    }

    public float strength = 0.1f;
    public float duration = 0.5f;
    public int vibrato = 10;
    private void StartPunchAnimation()
    {
        target.DOComplete();
        target.localScale = localScale;
        target.DOPunchScale(Vector3.one * strength, duration, vibrato)
                 .SetUpdate(true)
                 .SetLink(gameObject);
    }
}