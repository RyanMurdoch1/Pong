using UnityEngine;

public class Paddle : MonoBehaviour, IRestrainedVertical
{
    [SerializeField] private Transform top = default, bottom = default;
    [SerializeField] private KeyCode upKey = default, downKey = default;
    [SerializeField] private float movementSpeed = default;
    public Transform ObjectTransform { get; private set; }
    public float MovementSpeed => movementSpeed;
    private Camera _camera;
    private float _paddleScreenYPosition;
    private RestrainedVerticalMovementController _movementController;

    private void Start()
    {
        _camera = Camera.main;
        if (_camera is null) return;
        GetStartPosition();
        _movementController = new RestrainedVerticalMovementController(this);
    }

    private void Update()
    {
        _paddleScreenYPosition = _camera.WorldToScreenPoint(ObjectTransform.position).y;
        CheckForPlayerInput();
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetKey(upKey))
        {
            _movementController.AttemptMoveUp(_paddleScreenYPosition, Time.deltaTime);
        }
        else if (Input.GetKey(downKey))
        {
            _movementController.AttemptMoveDown(_paddleScreenYPosition, Time.deltaTime);
        }
    }

    public float GetObjectPixelHeight()
    {
        return _camera.WorldToScreenPoint(top.position).y - _camera.WorldToScreenPoint(bottom.position).y;
    }

    public float GetScreenHeightInPixels()
    {
        return Screen.height;
    }

    public Vector3 GetStartPosition()
    {
        ObjectTransform = transform;
        return ObjectTransform.localPosition;
    }
}