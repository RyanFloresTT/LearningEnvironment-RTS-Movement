using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RTSController : MonoBehaviour
{
    [SerializeField] private GameObject selectionArea;

    private Vector3 startPosition;
    private PlayerInputActions playerInputActions;
    private Camera mainCamera;
    private List<PlayerControllableUnit> selectedUnits;
    private Formations formation;
    private FormationDropdownMenu formationDropdownMenu;

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

        playerInputActions.RTSController.MoveTowards.performed += Handle_MoveTowards_Performed;

        playerInputActions.RTSController._2DOverlay.Enable();
        playerInputActions.RTSController.MoveTowards.Enable();
    }

    private void Handle_FormationChanged(object sender, Formations e)
    {
        formation = e;
    }

    private void OnDisable()
    {
        playerInputActions.RTSController._2DOverlay.Disable();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        formationDropdownMenu = FormationDropdownMenu.Instance;
        formationDropdownMenu.OnFormationDropdownMenuChanged += Handle_FormationChanged;
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

    private void Handle_MoveTowards_Performed(InputAction.CallbackContext obj)
    {
        switch(formation)
        {
            case Formations.HorizontalLine:
                MoveTowardsHorizontalLine();
                break;
            case Formations.VerticalLine:
                MoveTowardsVerticalLine();
                break;
            case Formations.Circle: 
                break;
        }
    }

    private Vector3 GetMouseToWorldPosition()
    {
        var mousePos = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void MoveTowardsHorizontalLine()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].MoveTo(GetMouseToWorldPosition() + new Vector3(0 + (1 * i), 0, 0));
        }
    }

    private void MoveTowardsVerticalLine()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].MoveTo(GetMouseToWorldPosition() + new Vector3(0 , 0 + (1 * i), 0));
        }
    }

    private void SetFormation(Formations formation)
    {
        this.formation = formation;
    }

    private bool SelectionAreaIsActive() => selectionArea.activeInHierarchy;

}
