using UnityEngine;

public class Paddle : MonoBehaviour, IRestrainedVerticalMovement
{
    [SerializeField] private Transform top = default, bottom = default;
    [SerializeField] private KeyCode upKey = default, downKey = default;
    [SerializeField] private float movementSpeed = default;
    public Transform ObjectTransform => transform;
    public float MovementSpeed => movementSpeed;
    private RestrainedVerticalMovementController _movementController;

    private void Start()
    {
        _movementController = new RestrainedVerticalMovementController(this);
    }

    private void Update()
    {
        CheckForPlayerInput();
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetKey(upKey))
        {
            _movementController.AttemptMoveUp(ScreenViewHandler.ReturnScreenYPosition(ObjectTransform.position), Time.deltaTime);
        }
        else if (Input.GetKey(downKey))
        {
            _movementController.AttemptMoveDown(ScreenViewHandler.ReturnScreenYPosition(ObjectTransform.position), Time.deltaTime);
        }
    }

    public float GetObjectPixelHeight() => ScreenViewHandler.ReturnScreenDistance(top.position, bottom.position);

    public float GetScreenHeightInPixels() => ScreenViewHandler.ReturnScreenHeight();
}