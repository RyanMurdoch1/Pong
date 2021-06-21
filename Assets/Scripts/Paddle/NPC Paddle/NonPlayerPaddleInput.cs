using System;
using UnityEngine;

public class NonPlayerPaddleInput : MonoBehaviour
{
    [SerializeField] private Paddle paddle = default;
    private const float ReachedTargetTolerance = 0.01f;
    private RectTransform _rectTransform;
    private float _requiredYPosition;
    
    private void Start()
    {
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
            paddle.AttemptMoveUp();
        }
        else if (_rectTransform.localPosition.y > _requiredYPosition)
        {
            paddle.AttemptMoveDown();
        }
    }
    
    private bool IsNotAtRequiredPosition() => Math.Abs(_requiredYPosition - _rectTransform.localPosition.y) > ReachedTargetTolerance;

    private void OnDisable() => Ball.DirectionChanged -= CheckRequiredPosition;
}