using UnityEngine;

public class BallMovementController
{
    private readonly IBall _ball;
    private readonly Camera _camera;
    private readonly Vector2 _viewScreenSize;
    private readonly float _viewScreenHeight;
    private Vector3 _ballScreenPosition => _camera.WorldToScreenPoint(_ball.RectTransform.position);


    public BallMovementController(IBall ball, Transform top, Transform bottom)
    {
        _ball = ball;
        _camera = Camera.main;
        if (_camera is null) return;
        _viewScreenSize = new Vector2(Screen.width, Screen.height);
        _viewScreenHeight = _camera.WorldToScreenPoint(top.position).y - _camera.WorldToScreenPoint(bottom.position).y;
    }
    
    public void UpdateBallLocalPosition(float timeStep)
    {
        var movementModifier = timeStep * _ball.MovementSpeed;
        _ball.RectTransform.localPosition += _ball.CurrentDirection * movementModifier;
    }
    
    public void CheckForScoring()
    {
        if (BallExitedScreenRight())
        {
            _ball.ScoredPoint(Player.PlayerOne);
        }
        else if (BallExitedScreenLeft())
        {
            _ball.ScoredPoint(Player.PlayerTwo);
        }
    }

    private bool BallExitedScreenLeft() => _ballScreenPosition.x < 0;

    private bool BallExitedScreenRight() => _ballScreenPosition.x > Screen.width;

    public void CheckForWallCollision()
    {
        CheckForTopCollision();
        CheckForBottomCollision();
    }
    
    public void CheckForPaddleCollision(RectTransform paddleRectTransform)
    {
        if (Helper.RectOverlaps(_ball.RectTransform, paddleRectTransform))
        {
            _ball.CurrentDirection = (_ball.RectTransform.localPosition - paddleRectTransform.localPosition).normalized;
        }
    }
    
    private void CheckForTopCollision()
    {
        if (_ballScreenPosition.y + _viewScreenHeight / 2 > _viewScreenSize.y)
        {
            _ball.CurrentDirection = Vector3.Reflect(_ball.CurrentDirection.normalized, Vector3.down);
        }
    }

    private void CheckForBottomCollision()
    {
        if (_ballScreenPosition.y - _viewScreenHeight / 2 < 0)
        {
            _ball.CurrentDirection = Vector3.Reflect(_ball.CurrentDirection.normalized, Vector3.up);
        }
    }
}