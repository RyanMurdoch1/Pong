using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    private const float  MaxYServeValue = 1.5f;
    private const int ServeRightX = 1;
    private const int ServeLeftX = -1;

    [SerializeField] private float ballSpeed = default;
    [SerializeField] private RectTransform top = default, bottom = default;
    [SerializeField] private RectTransform playerOnePaddle = default, playerTwoPaddle = default;

    public Vector3 CurrentDirection { get; set; }
    public RectTransform RectTransform
    {
        get => _ballRect;
        set => _ballRect = value;
    }
    public float MovementSpeed => ballSpeed;

    private BallMovementController _movementController;
    private RectTransform _ballRect;
    private Vector3 _ballScreenPosition;
    private readonly WaitForSeconds _serveWaitInSeconds = new WaitForSeconds(1f);

    private void Start()
    {
        _ballRect = GetComponent<RectTransform>();
        _movementController = new BallMovementController(this, top, bottom);
        StartCoroutine(ServeBall(Player.PlayerOne));
    }

    private void Update()
    {
        _movementController.UpdateBallLocalPosition(Time.deltaTime);
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
        ResetBallPosition();
        yield return _serveWaitInSeconds;
        ServeTowards(playerToServeTo);
    }
    
    // move to mvement
    private void ResetBallPosition()
    {
        _ballRect.transform.localPosition = Vector3.zero;
        CurrentDirection = Vector3.zero;
    }
    
    private void ServeTowards(Player playerToServeTo)
    {
        var x = playerToServeTo == Player.PlayerOne ? ServeRightX : ServeLeftX;
        var y = Random.Range(-MaxYServeValue, MaxYServeValue);
        CurrentDirection = new Vector3(x, y);
    }
}