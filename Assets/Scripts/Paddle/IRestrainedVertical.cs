using UnityEngine;

public interface IRestrainedVerticalMovement
{
    float GetObjectPixelHeight();
    float GetScreenHeightInPixels();
    Transform ObjectTransform { get; }
    float MovementSpeed { get; }
}