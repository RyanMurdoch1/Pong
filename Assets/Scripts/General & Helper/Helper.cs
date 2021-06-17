using UnityEngine;

public class Helper
{
    private const float PlanarFactorThreshold = 0.0001f;
    private const float VectorSquareMagnitudeThreshold = 0.0001f;

    public static bool RectOverlaps(RectTransform rectTransformOne, RectTransform rectTransformTwo)
    {
        return GenerateRectFromLocal(rectTransformOne).Overlaps(GenerateRectFromLocal(rectTransformTwo));
    }

    public static float GetRequiredYPosition(Vector3 objectPosition, Vector3 ballPosition, Vector2 ballDirection)
    {
        if (BallIsMovingAwayFromObject(objectPosition, ballPosition, ballDirection))
        {
            return objectPosition.y;
        }
        
        if (BallTrajectoryIsOverObject(objectPosition.y, ballPosition, ballDirection))
        {
            return LineIntersectionPoint(ballPosition,ballDirection, objectPosition, Vector3.up).y;
        }
        
        return BallTrajectoryIsUnderObject(objectPosition.y, ballPosition, ballDirection) ? LineIntersectionPoint(ballPosition,ballDirection, objectPosition, Vector3.down).y : objectPosition.y;
    }

    private static bool BallIsMovingAwayFromObject(Vector3 objectPosition, Vector3 ballPosition, Vector2 ballDirection)
    {
        return ballPosition.x < objectPosition.x && ballDirection.x < 0 || ballPosition.x > objectPosition.x && ballDirection.x > 0;
    }

    private static bool BallTrajectoryIsUnderObject(float localY, Vector3 ballPosition, Vector2 ballDirection)
    {
        return ballDirection.y < 0 || ballPosition.y > localY;
    }

    private static bool BallTrajectoryIsOverObject(float localY, Vector3 ballPosition, Vector2 ballDirection)
    {
        return ballDirection.y > 0 || ballPosition.y > localY;
    }

    private static Vector3 LineIntersectionPoint( Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {
        var lineVec3 = linePoint2 - linePoint1;
        var crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        var crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
        var planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
        if (!PlanarUnderThresholdAndCrossMagnitudeOverThreshold(planarFactor, crossVec1and2)) return Vector3.zero;
        var s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
        var intersectionPoint = linePoint1 + (lineVec1 * s);
        return intersectionPoint;
    }

    private static bool PlanarUnderThresholdAndCrossMagnitudeOverThreshold(float planarFactor, Vector3 crossVec1and2)
    {
        return Mathf.Abs(planarFactor) < PlanarFactorThreshold && crossVec1and2.sqrMagnitude > VectorSquareMagnitudeThreshold;
    }

    private static Rect GenerateRectFromLocal(RectTransform localRectTransform)
    {
        var rectHeight = localRectTransform.rect.height;
        return new Rect(localRectTransform.localPosition.x, localRectTransform.localPosition.y - rectHeight / 2, localRectTransform.rect.width, rectHeight);
    }
}