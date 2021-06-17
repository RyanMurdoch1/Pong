using System;
using UnityEngine;

public class NPCPaddle : MonoBehaviour, IRestrainedVerticalMovement
{
    private const float ReachedTargetTolerance = 0.01f;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private float movementSpeed = default;
    public Transform ObjectTransform => transform;
    private RectTransform _rectTransform;
    private RestrainedVerticalMovementController _movementController;
    private float _requiredYPosition;

    private void Start()
    {
        _movementController = new RestrainedVerticalMovementController(this);
        _rectTransform = GetComponent<RectTransform>();
        Ball.DirectionChanged += CheckRequiredPosition;
    }

    private void Update()
    {
        if (IsNotAtRequiredPosition())
        {
            MoveToRequiredPosition();
        }
    }

    private void CheckRequiredPosition(Vector3 ballPosition, Vector2 ballDirection)
    {
        _requiredYPosition = Helper.GetRequiredYPosition(_rectTransform.localPosition, ballPosition, ballDirection);
    }

    private void MoveToRequiredPosition()
    {
        if (_rectTransform.localPosition.y < _requiredYPosition)
        {
            _movementController.AttemptMoveUp(ScreenViewHandler.ReturnScreenYPosition(ObjectTransform.position), Time.deltaTime);
        }
        else if (_rectTransform.localPosition.y > _requiredYPosition)
        {
            _movementController.AttemptMoveDown(ScreenViewHandler.ReturnScreenYPosition(ObjectTransform.position), Time.deltaTime);
        }
    }

    private void OnDisable() => Ball.DirectionChanged -= CheckRequiredPosition;

    private bool IsNotAtRequiredPosition() => Math.Abs(_requiredYPosition - _rectTransform.localPosition.y) > ReachedTargetTolerance;

    public float GetObjectPixelHeight() => ScreenViewHandler.ReturnScreenDistance(top.position, bottom.position);

    public float GetScreenHeightInPixels() => ScreenViewHandler.ReturnScreenHeight();

    public float MovementSpeed => movementSpeed;
}