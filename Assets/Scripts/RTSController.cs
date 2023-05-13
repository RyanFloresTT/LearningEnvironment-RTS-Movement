using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RTSController : MonoBehaviour
{
    [SerializeField] private GameObject selectionArea;

    private Vector3 startPosition;
    private PlayerInputActions playerInputActions;
    private Camera mainCamera;
    private List<PlayerControllableUnit> selectedUnits;

    private void Awake()
    { 
        playerInputActions = new();
        selectedUnits = new();

        selectionArea.SetActive(false);
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

    private void Update()
    {
        if (SelectionAreaIsActive())
        {
            Vector3 mousePos = GetMouseToWorldPosition();
            Vector3 lowerLeft = new(
                Mathf.Min(startPosition.x, mousePos.x),
                Mathf.Min(startPosition.y, mousePos.y) );
            Vector3 upperRight = new(
                Mathf.Max(startPosition.x, mousePos.x),
                Mathf.Max(startPosition.y, mousePos.y));

            selectionArea.transform.position = lowerLeft;
            selectionArea.transform.localScale = upperRight - lowerLeft;
        }
    }

    private void Handle_Overlay_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        startPosition = GetMouseToWorldPosition();
        selectionArea.SetActive(true);
    }

    private void Handle_Overlay_Canceled(InputAction.CallbackContext obj)
    {
        selectionArea.SetActive(false);

        var endPosition = GetMouseToWorldPosition();
        var objectArray = Physics2D.OverlapAreaAll(startPosition, endPosition);

        foreach (PlayerControllableUnit unit in selectedUnits)
        {
            unit.DeselectUnit();
        }

        selectedUnits.Clear();

        foreach (Collider2D collider2D in objectArray)
        {
            PlayerControllableUnit controllableUnit = collider2D.GetComponent<PlayerControllableUnit>();
            if (controllableUnit == null) return;
            selectedUnits.Add(controllableUnit);
            controllableUnit.SelectUnit();
        }

        Debug.Log(selectedUnits.Count);
    }

    private Vector3 GetMouseToWorldPosition()
    {
        var mousePos = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private bool SelectionAreaIsActive() => selectionArea.activeInHierarchy;

}
