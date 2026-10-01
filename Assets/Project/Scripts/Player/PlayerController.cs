using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAttributes))] // Automatically adds attributes if missing
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float sprintSpeed = 8.5f; // Absolute speed when sprinting

    [Header("Look")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minPitch = -85f;
    [SerializeField] private float maxPitch = 85f;

    private CharacterController characterController;
    private PlayerAttributes attributes;
    private Vector3 verticalVelocity;
    private float cameraPitch = 0f;
    //public bool to see if player is moving
    public bool IsMoving => characterController.velocity.magnitude > 0.1f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        attributes = GetComponent<PlayerAttributes>();
    }
    /// <summary>
    ///  on start, lock the cursor in place and make it invisible
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // each update tick call the movement and roation functions
    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Yaw: rotate the entire player body horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Pitch: rotate only the camera vertically, clamped
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        cameraRoot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
    // movement up/down/left/right/front/back
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Check if player wants to sprint, is moving forward, is grounded, AND has stamina left
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && vertical > 0f && characterController.isGrounded;
        bool isSprinting = wantsToSprint && attributes.HasStamina;
        // drain stamina if they are holding shift
        if (isSprinting)
        {
            attributes.DrainStaminaForSprint();
        }

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Ground check & jumping
        if (characterController.isGrounded)
        {
            if (verticalVelocity.y < 0f)
            {
                verticalVelocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && attributes.HasStamina && (attributes.CurrentStamina > attributes.StaminaLossOnJump)) // if they can jump
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                attributes.DrainStaminaForJump();
            }
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 totalVelocity = (moveDirection * currentSpeed) + verticalVelocity;
        characterController.Move(totalVelocity * Time.deltaTime);
    }
}