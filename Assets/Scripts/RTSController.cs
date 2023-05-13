using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RTSController : MonoBehaviour
{
    private Vector3 startPosition;
    private PlayerInputActions playerInputActions;
    private Camera mainCamera;

    private void Awake()
    { 
        playerInputActions = new();
    }

    private void OnEnable()
    {
        playerInputActions.RTSController._2DOverlay.performed += Handle_Overlay_Performed;
        playerInputActions.RTSController._2DOverlay.canceled += Handle_Overlay_Canceled;

        playerInputActions.RTSController._2DOverlay.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.RTSController._2DOverlay.Disable();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Handle_Overlay_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        startPosition = GetMouseToWorldPosition();
    }

    private void Handle_Overlay_Canceled(InputAction.CallbackContext obj)
    {
        var endPosition = GetMouseToWorldPosition();
        var objectArray = Physics2D.OverlapAreaAll(startPosition, endPosition);
        Debug.Log("###");
        foreach (Collider2D collider2D in objectArray)
        {
            Debug.Log(collider2D);
        }
    }

    private Vector3 GetMouseToWorldPosition()
    {
        var mousePos = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

}
