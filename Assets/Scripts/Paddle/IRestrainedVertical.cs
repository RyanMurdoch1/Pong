using UnityEngine;

public interface IRestrainedVertical
{
    float GetObjectPixelHeight();
    float GetScreenHeightInPixels();
    Vector3 GetStartPosition();
    Transform ObjectTransform { get; }
    float MovementSpeed { get; }
}