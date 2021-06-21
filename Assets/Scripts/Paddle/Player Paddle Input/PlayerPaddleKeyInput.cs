using UnityEngine;

public class PlayerPaddleKeyInput : MonoBehaviour
{
    [SerializeField] private Paddle paddle = default;
    [SerializeField] private KeyCode upKey = default, downKey = default;
    
    private void Update()
    {
        CheckForPlayerInput();
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetKey(upKey))
        {
            paddle.AttemptMoveUp();
        }
        else if (Input.GetKey(downKey))
        {
            paddle.AttemptMoveDown();
        }
    }
}