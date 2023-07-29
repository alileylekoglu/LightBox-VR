using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class ObjectCloning : MonoBehaviour
{
    // You can assign the object you want to clone in the inspector or via script
    public GameObject objectToClone;

    // This variable will hold the reference to the cloned object
    private GameObject clonedObject;

    // Reference to the grip button InputAction
    public InputActionReference gripAction;

    private void OnEnable()
    {
        // Start listening to the grip action
        gripAction.action.started += OnGripPressed;
        gripAction.action.Enable();
    }

    private void OnDisable()
    {
        // Stop listening to the grip action
        gripAction.action.started -= OnGripPressed;
        gripAction.action.Disable();
    }

    private void OnGripPressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Clone the object when the grip button is pressed
        CloneObject();
    }

    private void CloneObject()
    {
        if (objectToClone != null)
        {
            // Clone the object and store the reference in clonedObject
            clonedObject = Instantiate(objectToClone, objectToClone.transform.position, objectToClone.transform.rotation);

            // Optionally, you can set the cloned object as a child of the controller
            // clonedObject.transform.parent = this.transform;
        }
        else
        {
            Debug.LogError("No object assigned to clone.");
        }
    }
}
