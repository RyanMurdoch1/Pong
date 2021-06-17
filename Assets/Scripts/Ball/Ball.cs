using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    [SerializeField] private float ballSpeed = default;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private RectTransform playerOnePaddle = default, playerTwoPaddle = default;
    public delegate void ChangedDirection(Vector3 localPosition, Vector2 newDirection);
    public static event ChangedDirection DirectionChanged;
    public Vector3 CurrentDirection { get; set; }
    public RectTransform RectTransform
    {
        get => _ballRect;
        set => _ballRect = value;
    }
    private BallMovementController _movementController;
    private RectTransform _ballRect;
    private Vector3 _ballScreenPosition;
    private readonly WaitForSeconds _serveWaitInSeconds = new WaitForSeconds(1f);

    private void Start()
    {
        _ballRect = GetComponent<RectTransform>();
        _movementController = new BallMovementController(this);
        StartCoroutine(ServeBallTo(Player.PlayerOne));
    }

    private void Update()
    {
        _movementController.UpdateBallLocalPosition(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _movementController.CheckForPaddleCollision(playerOnePaddle);
        _movementController.CheckForPaddleCollision(playerTwoPaddle);
        _movementController.CheckForWallCollision();
        _movementController.CheckForScoring();
    }

    public void ScoredPoint(Player scoringPlayer)
    {
        var playerToServe = scoringPlayer == Player.PlayerOne ? Player.PlayerTwo : Player.PlayerOne;
        StartCoroutine(ServeBallTo(playerToServe));
    }

    public float ReturnBallScreenHeight() => ScreenViewHandler.ReturnScreenDistance(top.position, bottom.position);

    public Vector2 ReturnViewScreenSize() => ScreenViewHandler.ReturnScreenDimensions();

    public void NewDirection() => DirectionChanged?.Invoke(_ballRect.localPosition, CurrentDirection);

    public float MovementSpeed() => ballSpeed;
    
    public Vector3 BallScreenPosition() => ScreenViewHandler.ReturnScreenPosition(_ballRect.position);

    private IEnumerator ServeBallTo(Player playerToServeTo)
    {
        _movementController.ResetBallPosition();
        yield return _serveWaitInSeconds;
        _movementController.ServeTowards(playerToServeTo);
    }
}