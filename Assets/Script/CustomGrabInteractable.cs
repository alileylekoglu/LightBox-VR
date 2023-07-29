using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrabInteractable : XRGrabInteractable
{
    public bool ScaleModeActive { get; set; } = false;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (ScaleModeActive)
        {
            Debug.Log("Object grabbed, scale: " + transform.localScale);
        }

        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (ScaleModeActive)
        {
            Debug.Log("Object released, scale: " + transform.localScale);
        }

        base.OnSelectExiting(args);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (!ScaleModeActive)
        {
            base.ProcessInteractable(updatePhase);
        }
    }
}
