using UnityEngine;

public class PlayerControllableUnit : MonoBehaviour
{
    [SerializeField] private GameObject border;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedMultiplier;
    private Vector3 targetPos;

    private void Awake()
    {
        border.SetActive(false);
        targetPos = transform.position;
    }

    public void SelectUnit()
    {
        border.SetActive(true);
    }

    public void DeselectUnit()
    {
        border.SetActive(false);
    }

    private void Update()
    {
        MoveToTargetPosition();
    }

    private void MoveToTargetPosition()
    {
        if (IsNotAtTargetPosition())
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * moveSpeedMultiplier * Time.deltaTime);
        }
    }

    public void MoveTo(Vector3 pos)
    {
        targetPos = pos;
    }

    private bool IsNotAtTargetPosition() => transform.position != targetPos;
}
