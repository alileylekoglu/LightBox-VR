using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimationController : MonoBehaviour
{
    public float z = 0f; // Z position of UI elements
    public float y = 0f; // Y position of UI elements
    public float x = 0f; // X position of UI elements

    public RectTransform[] uiElements; // Array of UI elements to animate
    public float animationDuration = 1f; // Duration of animation in seconds
    public Ease animationEase = Ease.OutQuad; // Type of animation ease
    public float startOpacity = 0f; // Starting opacity of UI elements
    public float endOpacity = 1f; // Ending opacity of UI elements

    private bool isAnimating = false; // Flag to prevent multiple animations at once

    public void PlayAnimation()
    {
        if (isAnimating) return; // Prevent multiple animations at once
        isAnimating = true;

        foreach (RectTransform uiElement in uiElements)
        {
            // Animate the opacity of the UI element using DoTween Pro
            CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = uiElement.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = startOpacity;
            canvasGroup.DOFade(endOpacity, animationDuration);

            // Animate the local position of the UI element using DoTween Pro
            Vector3 startPosition = uiElement.localPosition;
            Vector3 endPosition = uiElement.localPosition + new Vector3(x, y, z);
            uiElement.localPosition = startPosition;
            uiElement.DOLocalMove(endPosition, animationDuration)
                .SetEase(animationEase)
                .OnComplete(() =>
                {
                    // Set flag to allow future animations
                    isAnimating = false;
                    // Destroy the CanvasGroup component to avoid memory leak
                    Destroy(canvasGroup);
                });
        }
    }
}
