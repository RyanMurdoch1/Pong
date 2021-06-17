using System;
using UnityEngine;

public class NPCPaddle : MonoBehaviour, IRestrainedVertical
{
    private const float ReachedTargetTolerance = 0.01f;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private float movementSpeed = default;
    public Transform ObjectTransform { get; private set; }
    private Camera _camera;
    private RectTransform _rectTransform;
    private RestrainedVerticalMovementController _movementController;
    private float _paddleScreenYPosition;
    private Vector3 _intersection;
    private float _requiredYPosition;
    private Vector3 _paddleTargetDirection;

    private void Start()
    {
        SetUpPaddle();
    }
    
    private void Update()
    {
        if (IsNotAtRequiredPosition())
        {
            MoveToRequiredPosition();
        }
    }
    
    // Don't like this
    public Vector3 GetStartPosition()
    {
        ObjectTransform = transform;
        return ObjectTransform.localPosition;
    }

    private void CheckRequiredPosition(Vector3 ballPosition, Vector2 ballDirection)
    {
        _requiredYPosition = Helper.GetRequiredYPosition(_rectTransform.localPosition, ballPosition, ballDirection);
    }

    private void MoveToRequiredPosition()
    {
        _paddleScreenYPosition = _camera.WorldToScreenPoint(ObjectTransform.position).y;

        if (_rectTransform.localPosition.y < _requiredYPosition)
        {
            _movementController.AttemptMoveUp(_paddleScreenYPosition, Time.deltaTime);
        }
        else if (_rectTransform.localPosition.y > _requiredYPosition)
        {
            _movementController.AttemptMoveDown(_paddleScreenYPosition, Time.deltaTime);
        }
    }
    
    private void SetUpPaddle()
    {
        _camera = Camera.main;
        if (_camera is null) return;
        GetStartPosition();
        _movementController = new RestrainedVerticalMovementController(this);
        _rectTransform = GetComponent<RectTransform>();
        Ball.DirectionChanged += CheckRequiredPosition;
    }

    private void OnDisable() => Ball.DirectionChanged -= CheckRequiredPosition;

    private bool IsNotAtRequiredPosition() => Math.Abs(_requiredYPosition - _rectTransform.localPosition.y) > ReachedTargetTolerance;

    float IRestrainedVertical.GetObjectPixelHeight() => _camera.WorldToScreenPoint(top.position).y - _camera.WorldToScreenPoint(bottom.position).y;

    public float GetScreenHeightInPixels() => Screen.height;

    public float MovementSpeed => movementSpeed;
}