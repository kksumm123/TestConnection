using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DORotationController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 rotationValue = new Vector3(0, 0, 10);
    [SerializeField] private WoonyMethods.CustomEase rotationEase = new WoonyMethods.CustomEase(0.5f);
    [SerializeField] private float waitTime = 3;
    private Sequence _handle;

    private void Awake()
    {
        WoonyMethods.Asserts(this, (target, nameof(target)));
    }

    private void OnEnable()
    {
        DisableTween();

        _handle = DOTween.Sequence()
            .SetLink(gameObject)
            .SetUpdate(true)
            .SetLoops(-1, LoopType.Restart);

        _handle.Append(
            target.DOLocalRotate(
                rotationValue,
                rotationEase.duration)
             .SetEase(rotationEase.customEase));
        _handle.AppendInterval(waitTime);
    }

    private void OnDisable()
    {
        DisableTween();
    }

    private void DisableTween()
    {
        _handle.Kill();
        target.localRotation = Quaternion.identity;
    }
}
