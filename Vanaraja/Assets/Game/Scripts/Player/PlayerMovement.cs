using Cinemachine;
using UnityEngine;
using RDG;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;

    public float startPosition = 0f;
    public int distance = 0;
    public int previousDistance = 0;

    public bool isMoving = false;
    public bool isWolf = false;

    [Header("Inputs")]
    public PlayerInput playerInput;

    [Header("Movement")]
    public float jumpSpeed = 10f;
    public float maxSpeed = 10;
    public float forwardSpeed = 10;
    public float sideSpeed = 10;
    public float downForce = 10f;
    public Vector3 refVelocity;
    public AnimationCurve speedCurve;
    bool jumped = false;

    public float groundTimer = 0f;
    public float groundCheckTime = 0.1f;

    [Header("Graphics Movement")]
    public float swaySpeed = 10f;
    public float maxTiltAngle = 45f;
    public Transform playerGraphic;

    [Header("Human")]
    public CinemachineVirtualCamera humanCam;
    public MovementSpecification humanSpecification;
    public Animator humanAnimator;

    [Header("Wolf")]
    public CinemachineVirtualCamera wolfCam;
    public MovementSpecification wolfSpecification;
    public Animator wolfAnimator;
    public float tempJumpSpeed = 1f;
    public static System.Action<int> PlayerMoved;

    /// <summary>
    /// To be called whenever the player switches b/w human form
    /// and wolf form. 
    /// </summary>
    public static System.Action<bool> WolfSwitch;

    private void Start()
    {
        startPosition = transform.position.z;
    }

    public void Sway()
    {
        if (groundTimer >= 0)
        {
            Quaternion rotateY = Quaternion.AngleAxis(playerInput.moveDir * maxTiltAngle, Vector3.forward);
            playerGraphic.localRotation = Quaternion.Slerp(playerGraphic.localRotation, rotateY, swaySpeed * Time.deltaTime);
        }
        else
        {
            Quaternion rotateY = Quaternion.AngleAxis(0, Vector3.forward);
            playerGraphic.localRotation = Quaternion.Slerp(playerGraphic.localRotation, rotateY, swaySpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        humanAnimator.SetFloat("Speed", Mathf.Lerp(0, 6, rb.velocity.magnitude / 6));
        if (isMoving)
        {
            Sway();
            groundTimer -= Time.deltaTime;
            distance = (int)(transform.position.z - startPosition);
            if (previousDistance < distance)
            {
                PlayerMoved?.Invoke(distance - previousDistance);
                previousDistance = distance;
            }
        }

        if (playerInput.switchToWolf)
        {
            SwitchState();
            playerInput.switchToWolf = false;
        }
    }

    public void GamePaused(bool isPaused) => isMoving = !isPaused;

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Responsible for player player jump and movement
    /// </summary>
    public void Move2()
    {
        if (playerInput.jumpTimer > 0 && groundTimer > 0 && isWolf)
        {
            playerInput.jumpTimer = 0;
            groundTimer = 0f;
            Vector3 jumpForce = Vector3.up * jumpSpeed;
            rb.AddForce(jumpForce, ForceMode.Impulse);
            Debug.Log("jumped- " + Time.timeSinceLevelLoad);
        }
        if (groundTimer > 0)
        {
            Vector3 force = new(playerInput.moveDir * sideSpeed, rb.velocity.y, forwardSpeed * speedCurve.Evaluate(distance));
            rb.velocity = force;
        }
        else
        {
            refVelocity = rb.velocity;
            Vector3 force = new(playerInput.moveDir * 0, rb.velocity.y, forwardSpeed * speedCurve.Evaluate(distance));
            rb.velocity = Vector3.SmoothDamp(rb.velocity, force, ref refVelocity, 0.3f);
            //Vector3 force = new(playerInput.moveDir * 0, rb.velocity.y, forwardSpeed);
            //rb.velocity = force;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity -= downForce * Vector3.up;
        }
    }

    public void Move()
    {
        Vector3 force = new(playerInput.moveDir * sideSpeed, rb.velocity.y, forwardSpeed * speedCurve.Evaluate(distance));

        if (groundTimer > 0)
        {
            if (playerInput.jumpTimer > 0 && isWolf)
            {
                Vibration.Vibrate(100, 125);
                playerInput.jumpTimer = -1f;
                groundTimer = -1f;
                force = new Vector3(force.x, tempJumpSpeed, force.z);
                Debug.Log("Jumped");
            }
            rb.velocity = force;
        }
        else
        {
            refVelocity = rb.velocity;
            force = new(playerInput.moveDir * 0, rb.velocity.y, forwardSpeed * speedCurve.Evaluate(distance));
            rb.velocity = Vector3.SmoothDamp(rb.velocity, force, ref refVelocity, 0.1f);
        }
    }

    /// <summary>
    /// Switch between wolf and human
    /// </summary>
    public void SwitchState()
    {
        isWolf = !isWolf;
        if (isWolf)
        {
            capsuleCollider.height = 1;
            capsuleCollider.center = new Vector3(0f, -0.5f, 0f);
            wolfCam.Priority = 1;
            humanCam.Priority = 0;
        }
        else
        {
            capsuleCollider.height = 2;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            wolfCam.Priority = 0;
            humanCam.Priority = 1;
        }
    }

    public void SwitchState(bool state)
    {
        isWolf = state;
        if (state)
        {
            humanAnimator.gameObject.SetActive(false);
            wolfAnimator.gameObject.SetActive(true);
            capsuleCollider.height = 1;
            capsuleCollider.center = new Vector3(0f, -0.5f, 0f);
            wolfCam.Priority = 1;
            humanCam.Priority = 0;
        }
        else
        {
            humanAnimator.gameObject.SetActive(true);
            wolfAnimator.gameObject.SetActive(false);
            capsuleCollider.height = 2;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            wolfCam.Priority = 0;
            humanCam.Priority = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.Ground))
        {
            if (isWolf && groundTimer < -0.6f)
            {
                Vibration.Vibrate(100, 125);
            }
            groundTimer = groundCheckTime;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.Ground))
        {
            groundTimer = groundCheckTime;
        }
    }

    public void GameOver()
    {
        isMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void GameStarted()
    {
        isMoving = true;
        rb.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        SwitchForm.SwitchStates += SwitchState;
        GameManager.GameOver += GameOver;
        GameManager.GameStarted += GameStarted;
        GameManager.GamePaused += GamePaused;
    }

    private void OnDisable()
    {
        SwitchForm.SwitchStates -= SwitchState;
        GameManager.GameOver -= GameOver;
        GameManager.GameStarted -= GameStarted;
        GameManager.GamePaused -= GamePaused;
    }
}
