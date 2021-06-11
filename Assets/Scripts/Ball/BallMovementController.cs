using UnityEngine;

public class BallMovementController
{
    private readonly IBall _ball;

    public BallMovementController(IBall ball)
    {
        _ball = ball;
    }
    
    public void UpdateBallPosition(float timeStep)
    {
        var movementModifier = timeStep * _ball.MovementSpeed;
        _ball.RectTransform.localPosition += _ball.CurrentDirection * movementModifier;
    }
    
    public void CheckForScoring()
    {
        if (_ball.ScreenPosition.x > Screen.width)
        {
            _ball.ScoredPoint(Player.PlayerTwo);
        }
        else if (_ball.ScreenPosition.x < 0)
        {
            _ball.ScoredPoint(Player.PlayerOne);
        }
    }

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
        if (_ball.ScreenPosition.y + _ball.ScreenHeight / 2 > _ball.ViewScreenSize.y)
        {
            _ball.CurrentDirection = Vector3.Reflect(_ball.CurrentDirection.normalized, Vector3.down);
        }
    }

    private void CheckForBottomCollision()
    {
        if (_ball.ScreenPosition.y - _ball.ScreenHeight / 2 < 0)
        {
            _ball.CurrentDirection = Vector3.Reflect(_ball.CurrentDirection.normalized, Vector3.up);
        }
    }
}