using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float rotationSmoothTime = 0.1f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;  // Assign your main camera here in the Inspector

    [Header("Stumble Physics")]
    public float stumbleForce = 5f;
    public float recoverTime = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isStumbled = false;
    private float stumbleTimer;

    float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Auto-assign if not set
        }
    }

    void Update()
    {
        if (isStumbled)
        {
            HandleStumble();
            return;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        MovePlayer();
        JumpPlayer();
        ApplyGravity();
    }

    void MovePlayer()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Convert move direction relative to camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    void JumpPlayer()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Stumble(Vector3 forceDirection)
    {
        if (isStumbled) return;

        isStumbled = true;
        stumbleTimer = recoverTime;
        velocity = forceDirection * stumbleForce;
    }

    void HandleStumble()
    {
        stumbleTimer -= Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;

        if (stumbleTimer <= 0)
            isStumbled = false;
    }
}
