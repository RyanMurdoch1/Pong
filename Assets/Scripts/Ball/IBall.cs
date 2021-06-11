using UnityEngine;

public interface IBall
{
    Vector3 ScreenPosition { get; }
    Vector3 CurrentDirection { get; set; }
    RectTransform RectTransform { get; set; }
    Vector2 ViewScreenSize { get; }
    float ScreenHeight { get; }
    float MovementSpeed { get; }
    void ScoredPoint(Player scoringPlayer);
}