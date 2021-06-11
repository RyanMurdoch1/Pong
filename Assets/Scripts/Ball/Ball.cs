using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float MinDirectionValue = 0.1f, MaxDirectionValue = 1.5f;
    
    [SerializeField] private float ballSpeed = default;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private RectTransform[] paddleRects = default;

    private RectTransform _ballRect;
    private Camera _camera;
    private float _pixelHeight, _ballYScreenPosition, _screenHeight;
    private Vector3 _currentDirection = new Vector3(-1, 1, 0);

    private void Start()
    {
        _camera = Camera.main;
        if (_camera is null) return;
        _screenHeight = Screen.height;
        _pixelHeight = _camera.WorldToScreenPoint(top.position).y - _camera.WorldToScreenPoint(bottom.position).y;
        _ballRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateBallPosition();
        CheckForCollisions();
    }

    private void UpdateBallPosition()
    {
        var movementModifier = Time.deltaTime * ballSpeed;
        transform.localPosition += _currentDirection * movementModifier;
    }

    private void CheckForCollisions()
    {
        for (var i = 0; i < paddleRects.Length; i++)
        {
            CheckForPaddleCollision(paddleRects[i]);
        }
        _ballYScreenPosition = _camera.WorldToScreenPoint(_ballRect.position).y;
        CheckForTopCollision();
        CheckForBottomCollision();
    }

    private void CheckForTopCollision()
    {
        if (_ballYScreenPosition + _pixelHeight / 2 > _screenHeight)
        {
            _currentDirection = Vector3.Reflect(_currentDirection.normalized, Vector3.down);
        }
    }

    private void CheckForBottomCollision()
    {
        if (_ballYScreenPosition - _pixelHeight / 2 < 0)
        {
            _currentDirection = Vector3.Reflect(_currentDirection.normalized, Vector3.up);
        }
    }

    private void CheckForPaddleCollision(RectTransform paddleRect)
    {
        if (RectOverlaps(_ballRect, paddleRect))
        {
            _currentDirection = (_ballRect.localPosition - paddleRect.localPosition).normalized;
        }
    }
    
    private static bool RectOverlaps(RectTransform rectTransformOne, RectTransform rectTransformTwo)
    {
        var rectOne = new Rect(rectTransformOne.localPosition.x, rectTransformOne.localPosition.y, rectTransformOne.rect.width, rectTransformOne.rect.height);
        var rectTwo = new Rect(rectTransformTwo.localPosition.x, rectTransformTwo.localPosition.y, rectTransformTwo.rect.width, rectTransformTwo.rect.height);
        return rectOne.Overlaps(rectTwo);
    }
}
