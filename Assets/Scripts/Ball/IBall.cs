using UnityEngine;

public interface IBall
{
    Vector3 CurrentDirection { get; set; }
    RectTransform RectTransform { get; set; }
    float MovementSpeed { get; }
    void ScoredPoint(Player scoringPlayer);
}