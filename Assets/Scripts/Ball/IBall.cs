using System;
using UnityEngine;

public interface IBall
{
    Vector3 CurrentDirection { get; set; }
    RectTransform RectTransform { get; set; }
    float ReturnMovementSpeed();
    Vector3 BallScreenPosition();
    void ScoredPoint(Player scoringPlayer);
    float ReturnBallScreenHeight();
    Vector2 ReturnViewScreenSize();
    void NewDirection();
}