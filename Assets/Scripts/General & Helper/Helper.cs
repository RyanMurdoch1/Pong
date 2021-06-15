using UnityEngine;

public class Helper
{
    public static bool RectOverlaps(RectTransform rectTransformOne, RectTransform rectTransformTwo)
    {
        return GenerateRectFromLocal(rectTransformOne).Overlaps(GenerateRectFromLocal(rectTransformTwo));
    }

    private static Rect GenerateRectFromLocal(RectTransform localRectTransform)
    {
        var rectHeight = localRectTransform.rect.height;
        return new Rect(localRectTransform.localPosition.x, localRectTransform.localPosition.y - rectHeight / 2, localRectTransform.rect.width, rectHeight);
    }
}