using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OneTimeMoveObject : MonoBehaviour
{
    public Vector3 positionA;
    public Vector3 positionB;
    public float duration = 2.0f;

    // This method will be called when the UI button is clicked
    public void OnButtonClicked()
    {
        MoveToPositionB();
    }

    void MoveToPositionB()
    {
        Debug.Log("Moving to Position B");
        transform.DOLocalMove(positionB, duration).SetEase(Ease.InOutQuad);
    }
}
