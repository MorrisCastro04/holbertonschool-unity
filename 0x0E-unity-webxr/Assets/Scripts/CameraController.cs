using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSensitivity = 2f;

    public InputActionAsset MoveControlsAsset;
    private InputAction moveAction;

    private float yaw = 0f;
    private bool rotating = false;

    private void Awake()
    {
        moveAction = MoveControlsAsset.FindAction("Move");
        yaw = transform.eulerAngles.y;
    }

    private void Update()
    {
        // Movimiento con teclado/control
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.forward * input.y + transform.right * input.x;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Comenzar rotación si se mantiene clic derecho (Mouse Button 1)
        if (Mouse.current.rightButton.isPressed)
        {
            if (!rotating)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                rotating = true;
            }

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            yaw += mouseDelta.x * rotationSensitivity;
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }
        else
        {
            if (rotating)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                rotating = false;
            }
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
