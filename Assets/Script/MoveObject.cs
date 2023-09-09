using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MoveObject : MonoBehaviour
{
    public Vector3 positionA;
    public Vector3 positionB;
    public float duration = 2.0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        MoveToPositionAandB();
    }

    void MoveToPositionAandB()
    {
        Debug.Log("MoveToPositionAandB");
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOLocalMove(positionA, duration).SetEase(Ease.InOutQuad))
            .Append(transform.DOLocalMove(positionB, duration).SetEase(Ease.InOutQuad))
            .Append(transform.DOLocalMove(originalPosition, duration).SetEase(Ease.InOutQuad))
            .OnComplete(() =>
            {
                MoveToPositionAandB();
            });
    }
}
