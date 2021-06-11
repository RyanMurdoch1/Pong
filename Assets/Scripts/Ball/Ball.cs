using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    private const float  MaxYServeValue = 1.5f;
    
    [SerializeField] private float ballSpeed = default;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private RectTransform playerOnePaddle = default, playerTwoPaddle = default;

    public Vector3 ScreenPosition => _camera.WorldToScreenPoint(_ballRect.position);
    public Vector3 CurrentDirection { get; set; }
    public RectTransform RectTransform
    {
        get => _ballRect;
        set => _ballRect = value;
    }
    public Vector2 ViewScreenSize { get; private set; }
    public float ScreenHeight { get; private set; }
    public float MovementSpeed => ballSpeed;

    private BallMovementController _movementController;
    private RectTransform _ballRect;
    private Camera _camera;
    private Vector3 _ballScreenPosition;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

    private void Start()
    {
        _camera = Camera.main;
        if (_camera is null) return;
        ViewScreenSize = new Vector2(Screen.width, Screen.height);
        ScreenHeight = _camera.WorldToScreenPoint(top.position).y - _camera.WorldToScreenPoint(bottom.position).y;
        _ballRect = GetComponent<RectTransform>();
        _movementController = new BallMovementController(this);
        StartCoroutine(ServeBall(Player.PlayerOne));
    }

    private void Update()
    {
        _movementController.UpdateBallPosition(Time.deltaTime);
        _movementController.CheckForWallCollision();
        _movementController.CheckForPaddleCollision(playerOnePaddle);
        _movementController.CheckForPaddleCollision(playerTwoPaddle);
        _movementController.CheckForScoring();
    }
        
    public void ScoredPoint(Player scoringPlayer)
    {
        StartCoroutine(ServeBall(scoringPlayer));
    }
    
    private IEnumerator ServeBall(Player playerToServeTo)
    {
        _ballRect.transform.localPosition = Vector3.zero;
        CurrentDirection = Vector3.zero;
        yield return _waitForSeconds;
        var x = playerToServeTo == Player.PlayerOne ? -1 : 1;
        var y = Random.Range(-MaxYServeValue, MaxYServeValue);
        CurrentDirection = new Vector3(x, y);
    }
}