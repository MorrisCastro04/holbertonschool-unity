using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController; // Reference to the CharacterController
    public Camera playerCamera; // Reference to the player's camera
    public float speed = 3.0f; // Movement speed
    public float rotationSpeed = 5.0f; // Rotation speed of the player
    public float mouseSensitivity = 100f;  // Mouse sensitivity
    private float rotationX = 0f;  // Rotation around the X-axis (vertical)
    private float rotationY = 0f;  // Rotation around the Y-axis (horizontal)

    private Vector3 moveDirection;

    private void Start()
    {
        // Check headset status at the start
        HeadSetVerificator.isHeadsetConnected = XRSettings.isDeviceActive;
        
        // Initially lock the cursor if no headset is detected
        if (!HeadSetVerificator.isHeadsetConnected)
        {
            LockCursor();
        }
    }

    void Update()
    {
        // If the headset is not connected, allow movement and camera rotation with keyboard/mouse
        if (!HeadSetVerificator.isHeadsetConnected)
        {
            if(!BallBehaviour.disableMouseKeyboard)
            {
                HandleMouseKeyboardMovement();
                HandleCameraRotation();
            }
        }
        else
        {
            // Unlock the cursor when headset is connected
            UnlockCursor();
        }
    }

    // Method to handle keyboard/mouse movement
    private void HandleMouseKeyboardMovement()
    {
        // Get the horizontal and vertical axis input (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Get the camera's forward and right directions (ignoring Y axis for flat movement)
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Flatten the forward and right vectors (remove vertical movement, i.e., Y-axis)
        forward.y = 0f;
        right.y = 0f;

        // Normalize the vectors to avoid diagonal speed boost
        forward.Normalize();
        right.Normalize();

        // Calculate the movement direction relative to the camera's facing direction
        moveDirection = forward * moveZ + right * moveX;

        // Apply movement
        characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    // Method to handle camera rotation with the mouse
    private void HandleCameraRotation()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical camera rotation (around the X-axis)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);  // Limit vertical rotation to avoid camera flip
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // Apply vertical rotation

        // Horizontal player rotation (around the Y-axis)
        rotationY += mouseX;
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f); // Apply horizontal rotation to the player
    }

    // Lock the cursor
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor to the center of the screen
        Cursor.visible = false;  // Hide the cursor
    }

    // Unlock the cursor
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
        Cursor.visible = true;  // Make the cursor visible again
    }
}
