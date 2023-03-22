using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum InputType
    {
        Keyboard,
        Touch,
        Tilt
    };

    public enum MoveDirection
    {
        Left = -1,
        Forward = 0,
        Right = 1
    };

    public SettingsMenu settings;
    public InputType inputType = InputType.Keyboard;

    public float inputDelay = 0.05f;
    public float inputTimer = 0f;
    public float moveDir = 0;
    public float jumpTimer = 0f;
    public float jumpCountdown = 0.2f;
    public bool inputsActive = true;
    public bool switchToWolf = false;

    private void Start()
    {
        settings = FindObjectOfType<SettingsMenu>();
    }
    private void Update()
    {
        inputType = settings.usingTilt ? InputType.Tilt : InputType.Touch;
        if (!inputsActive)
        {
            return;
        }
        switch (inputType)
        {
            case InputType.Keyboard:
                KeyboardInput();
                break;
            case InputType.Touch:
                TouchInput();
                break;
            case InputType.Tilt:
                TiltInput(); break;
        }
    }

    private void Update1()
    {
        if (!inputsActive)
        {
            return;
        }
        switch (inputType)
        {
            case InputType.Keyboard:
                KeyboardInput();
                break;
            case InputType.Touch:
                TouchInput();
                break;
            case InputType.Tilt:
                TiltInput(); break;
        }
    }

    public void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switchToWolf = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jumpTimer = jumpCountdown;
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }
        moveDir = Input.GetAxisRaw("Horizontal");
    }

    public void TouchInput()
    {
        if (Input.touchCount >= 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                moveDir = 0f;
                jumpTimer = jumpCountdown;
            }
        }
        else if (Input.touchCount == 1)
        {
            inputTimer -= Time.deltaTime;
            if (inputTimer < 0)
            {
                Touch touch = Input.GetTouch(0);
                moveDir = touch.position.x < Screen.width / 2 ? -1f : 1f;
            }
        }
        else
        {
            inputTimer = inputDelay;
            jumpTimer -= Time.deltaTime;
            moveDir = 0f;
        }
    }

    public void TiltInput()
    {
        float temp = Input.acceleration.x;
        //if (Mathf.Abs(temp) > 0.1)
        //{
        //    moveDir = Mathf.Sign(temp);
        //}
        if (Mathf.Abs(temp) > 0.1)
        {
            moveDir = Mathf.Clamp(temp + 0.5f*Mathf.Sign(temp),-1f,1f);
        }
        else
        {
            moveDir = 0f;
        }
        if (Input.touchCount >= 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                moveDir = 0f;
                jumpTimer = jumpCountdown;
            }
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    public void GameOver() => inputsActive = false;
    public void GameStarted() => inputsActive = true;
    public void GamePaused(bool isPaused) => inputsActive = !isPaused;

    private void OnEnable()
    {
        GameManager.GameOver += GameOver;
        GameManager.GameStarted += GameStarted;
        GameManager.GamePaused += GamePaused;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= GameOver;
        GameManager.GameStarted -= GameStarted;
        GameManager.GamePaused -= GamePaused;
    }
}
