using UnityEngine;

public class RestrainedVerticalMovementController
{
    private readonly float _objectPixelHeight, _screenPixelHeight, _verticalMovementSpeed;
    private readonly Transform _movingTransform;
    private readonly Vector3 _startingPosition;

    private IRestrainedVertical _movementObject;
    
    public RestrainedVerticalMovementController(IRestrainedVertical movementObject)
    {
        _objectPixelHeight = movementObject.GetObjectPixelHeight();
        _movingTransform = movementObject.ObjectTransform;
        _startingPosition = movementObject.GetStartPosition();
        _screenPixelHeight = movementObject.GetScreenHeightInPixels();
        _verticalMovementSpeed = movementObject.MovementSpeed;
        _movementObject = movementObject;
    }

    public void AttemptMoveUp(float currentYPosition, float timeStep)
    {
        if (currentYPosition + _objectPixelHeight / 2 < _screenPixelHeight)
        {
            MoveObject(_movementObject.MovementSpeed, timeStep);
        }
    }

    public void AttemptMoveDown(float currentYPosition, float timeStep)
    {
        if (currentYPosition - _objectPixelHeight / 2 > 0)
        {
            MoveObject(-_movementObject.MovementSpeed, timeStep);
        }
    }
    
    private void MoveObject(float yMovementModifier, float timeStep)
    {
        var yPosition = _movingTransform.localPosition.y + yMovementModifier * timeStep;
        _movingTransform.localPosition = new Vector3(_startingPosition.x, yPosition, _startingPosition.z);
    }
}