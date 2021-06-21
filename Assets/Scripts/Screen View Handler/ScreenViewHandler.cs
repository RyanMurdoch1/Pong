using UnityEngine;

public class ScreenViewHandler : MonoBehaviour
{
    private static Camera _camera;
    private static Vector2 _screenDimensions;

    private void Awake()
    {
        _camera = Camera.main;
        _screenDimensions = new Vector2(Screen.width, Screen.height);
    }

    public static float ReturnScreenHeight() => _screenDimensions.y;

    public static Vector2 ReturnScreenDimensions() => _screenDimensions;

    public static Vector3 ReturnScreenPosition(Vector3 position) => _camera.WorldToScreenPoint(position);

    public static float ReturnScreenYPosition(Vector3 position) => _camera.WorldToScreenPoint(position).y;

    public static float ReturnScreenDistance(Vector3 top, Vector3 bottom) => ReturnScreenYPosition(top) - ReturnScreenYPosition(bottom);
}
