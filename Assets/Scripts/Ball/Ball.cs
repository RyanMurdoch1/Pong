using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    [SerializeField] private float ballSpeed = default;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private RectTransform playerOnePaddle = default, playerTwoPaddle = default;

    public Vector3 CurrentDirection { get; set; }
    public RectTransform RectTransform
    {
        get => _ballRect;
        set => _ballRect = value;
    }
    
    public float ReturnMovementSpeed() => ballSpeed;
    public Vector3 BallScreenPosition() => _camera.WorldToScreenPoint(RectTransform.position);

    private Camera _camera;
    private BallMovementController _movementController;
    private RectTransform _ballRect;
    private Vector3 _ballScreenPosition;
    private readonly WaitForSeconds _serveWaitInSeconds = new WaitForSeconds(1f);

    private void Start()
    {
        _ballRect = GetComponent<RectTransform>();
        _camera = Camera.main;
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
        StartCoroutine(ServeBallTo(scoringPlayer));
    }

    public float ReturnBallScreenHeight()
    {
        return _camera.WorldToScreenPoint(top.position).y - _camera.WorldToScreenPoint(bottom.position).y;
    }

    public Vector2 ReturnViewScreenSize()
    {
        return new Vector2(Screen.width, Screen.height);
    }

    private IEnumerator ServeBallTo(Player playerToServeTo)
    {
        _movementController.ResetBallPosition();
        yield return _serveWaitInSeconds;
        _movementController.ServeTowards(playerToServeTo);
    }
}