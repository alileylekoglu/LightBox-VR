using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System;

public class ScaleObject : MonoBehaviour
{
    private CustomGrabInteractable grabInteractable;
    private Vector3 originalScale;
    private List<InputDevice> devices = new List<InputDevice>();
    private const float ScaleSpeed = 0.1f;
    private float cumulativeScale = 1f;

    public float minScale;
    public float maxScale;

    void Awake()
    {
        grabInteractable = GetComponent<CustomGrabInteractable>();
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(StartScaling);
        grabInteractable.selectExited.AddListener(EndScaling);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(StartScaling);
        grabInteractable.selectExited.RemoveListener(EndScaling);
    }

    void StartScaling(SelectEnterEventArgs args)
    {
        cumulativeScale = 1f;
    }

    void EndScaling(SelectExitEventArgs args)
    {
        if (grabInteractable.ScaleModeActive)
        {
            originalScale = transform.localScale; // Save the new scale when ending the grab
            grabInteractable.ScaleModeActive = false;
        }
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Handle scale on primary button press
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, devices);
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
                {
                    grabInteractable.ScaleModeActive = true;
                }
            }

            if (grabInteractable.ScaleModeActive)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    if (devices[i].TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxis))
                    {
                        float scaleFactor = 1 + (primary2DAxis.y * ScaleSpeed);
                        scaleFactor = Mathf.Round(scaleFactor * 10f) / 10f; // Round to nearest tenth

                        cumulativeScale *= scaleFactor;
                        cumulativeScale = Mathf.Clamp(cumulativeScale, minScale, maxScale); // Limit scale to range minScale to maxScale

                        transform.localScale = originalScale * cumulativeScale;
                    }
                }
            }
        }
        else if (grabInteractable.ScaleModeActive)
        {
            // Handle the case where the object has been deselected but ScaleModeActive is still true
            originalScale = transform.localScale; // Save the new scale when ending the grab
            grabInteractable.ScaleModeActive = false;
        }
    }

}