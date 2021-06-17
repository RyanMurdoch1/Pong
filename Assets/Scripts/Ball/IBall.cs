using UnityEngine;

public interface IBall
{
    Vector3 CurrentDirection { get; set; }
    RectTransform RectTransform { get; set; }
    float MovementSpeed();
    Vector3 BallScreenPosition();
    void ScoredPoint(Player scoringPlayer);
    float ReturnBallScreenHeight();
    Vector2 ReturnViewScreenSize();
    void NewDirection();
}