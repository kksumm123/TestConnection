using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float minScale = 0.8f;
    [Range(1, 3)]
    [SerializeField] float maxScale = 1.2f;
    [Range(0, float.MaxValue)]
    [SerializeField] float duration = 1f;
    [SerializeField] bool setLoop = false;

    Vector3 originScale;
    void Start()
    {
        transform.localScale *= minScale;
        if (setLoop)
        {
            transform.DOScale(maxScale, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true)
                .SetLink(gameObject);
        }
        else
        {
            transform.DOScale(maxScale, duration)
                .SetUpdate(true)
                .SetLink(gameObject);
        }
    }
}
