using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float jumpHeight = 1.2f;

    [Header("Look")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minPitch = -85f;
    [SerializeField] private float maxPitch = 85f;

    private CharacterController characterController;
    private Vector3 verticalVelocity;
    private float cameraPitch = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
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
        // 1. Gather directional input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

        // 2. Handle ground state and jumping
        if (characterController.isGrounded)
        {
            // Keep a light downward force while grounded so isGrounded stays true
            if (verticalVelocity.y < 0f)
            {
                verticalVelocity.y = -2f;
            }

            // Trigger jump
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            // 3. Only apply gravity acceleration while airborne
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        // 4. Combine horizontal motion and vertical velocity into ONE single move call
        Vector3 totalVelocity = (moveDirection * walkSpeed) + verticalVelocity;
        characterController.Move(totalVelocity * Time.deltaTime);
    }
}