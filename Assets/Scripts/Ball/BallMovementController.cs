using UnityEngine;

public class BallMovementController
{
    private const float  MaxYServeValue = 1.5f;
    private const int ServeRightX = 1;
    private const int ServeLeftX = -1;
    private readonly float _ballScreenHeight;
    private readonly Vector2 _screenDimensions;
    
    private readonly IBall _ball;

    public BallMovementController(IBall ball)
    {
        _ball = ball;
        _ballScreenHeight = ball.ReturnBallScreenHeight();
        _screenDimensions = ball.ReturnViewScreenSize();
    }
    
    public void UpdateBallLocalPosition(float timeStep)
    {
        var movementModifier = timeStep * _ball.MovementSpeed();
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
    
    public void CheckForWallCollision()
    {
        CheckForTopCollision();
        CheckForBottomCollision();
    }
    
    public void CheckForPaddleCollision(RectTransform paddleRectTransform)
    {
        if (!Helper.RectOverlaps(_ball.RectTransform, paddleRectTransform)) return;
        _ball.CurrentDirection = (_ball.RectTransform.localPosition - paddleRectTransform.localPosition).normalized;
        _ball.NewDirection();
    }

    public void ResetBallPosition()
    {
        _ball.RectTransform.localPosition = Vector3.zero;
        _ball.CurrentDirection = Vector3.zero;
    }
    
    public void ServeTowards(Player playerToServeTo)
    {
        var x = playerToServeTo == Player.PlayerOne ? ServeLeftX : ServeRightX ;
        var y = Random.Range(-MaxYServeValue, MaxYServeValue);
        _ball.CurrentDirection = new Vector3(x, y);
        _ball.NewDirection();
    }
    
    private bool BallExitedScreenLeft() => _ball.BallScreenPosition().x < 0;

    private bool BallExitedScreenRight() => _ball.BallScreenPosition().x > _screenDimensions.x;

    private void CheckForTopCollision()
    {
        if (!IsMovingUpAndAtScreenTop()) return;
        _ball.CurrentDirection = Vector3.Reflect(_ball.CurrentDirection.normalized, Vector3.down);
        _ball.NewDirection();
    }

    private bool IsMovingUpAndAtScreenTop()
    {
        return _ball.BallScreenPosition().y + _ballScreenHeight / 2 >= _screenDimensions.y && _ball.CurrentDirection.y > 0;
    }

    private void CheckForBottomCollision()
    {
        if (!IsMovingDownAndAtScreenBottom()) return;
        _ball.CurrentDirection = Vector3.Reflect(_ball.CurrentDirection.normalized, Vector3.up);
        _ball.NewDirection();
    }

    private bool IsMovingDownAndAtScreenBottom()
    {
        return _ball.BallScreenPosition().y - _ballScreenHeight / 2 <= 0 && _ball.CurrentDirection.y < 0;
    }
}