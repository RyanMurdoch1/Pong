using UnityEngine;

public class Paddle : MonoBehaviour, IRestrainedVerticalMovement
{
    [SerializeField] private Transform top = default, bottom = default;
    [SerializeField] private float movementSpeed = default;
    public Transform ObjectTransform => transform;
    public float MovementSpeed => movementSpeed;
    private RestrainedVerticalMovementController _movementController;

    private void Start()
    {
        _movementController = new RestrainedVerticalMovementController(this);
    }
    
    public void AttemptMoveUp()
    {
        _movementController.AttemptMoveUp(ScreenViewHandler.ReturnScreenYPosition(ObjectTransform.position), Time.deltaTime);
    }

    public void AttemptMoveDown()
    {
        _movementController.AttemptMoveDown(ScreenViewHandler.ReturnScreenYPosition(ObjectTransform.position), Time.deltaTime);
    }
    
    public float GetObjectPixelHeight() => ScreenViewHandler.ReturnScreenDistance(top.position, bottom.position);

    public float GetScreenHeightInPixels() => ScreenViewHandler.ReturnScreenHeight();
}