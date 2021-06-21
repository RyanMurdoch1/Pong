using UnityEngine;

public class Helper
{
    private const float PlanarFactorThreshold = 0.0001f;
    private const float VectorSquareMagnitudeThreshold = 0.0001f;

    public static bool RectOverlaps(RectTransform rectTransformOne, RectTransform rectTransformTwo)
    {
        return GenerateRectFromLocal(rectTransformOne).Overlaps(GenerateRectFromLocal(rectTransformTwo));
    }

    private static Rect GenerateRectFromLocal(RectTransform localRectTransform)
    {
        var rectHeight = localRectTransform.rect.height;
        return new Rect(localRectTransform.localPosition.x, localRectTransform.localPosition.y - rectHeight / 2, localRectTransform.rect.width, rectHeight);
    }
    
    public static float GetRequiredYPosition(Vector3 objectPosition, Vector3 targetPosition, Vector2 targetDirection)
    {
        return TargetIsMovingAwayFromObject(objectPosition, targetPosition, targetDirection) ? objectPosition.y : LineIntersectionPoint(targetPosition,targetDirection, objectPosition).y;
    }

    private static bool TargetIsMovingAwayFromObject(Vector3 objectPosition, Vector3 targetPosition, Vector2 targetDirection)
    {
        return targetPosition.x < objectPosition.x && targetDirection.x < 0 || targetPosition.x > objectPosition.x && targetDirection.x > 0;
    }

    private static Vector3 LineIntersectionPoint( Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2)
    {
        var lineVec3 = linePoint2 - linePoint1;
        var crossVec1and2 = Vector3.Cross(lineVec1, Vector3.up);
        var crossVec3and2 = Vector3.Cross(lineVec3, Vector3.up);
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
}