using System.Collections;
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

    public IEnumerator CameraUP()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, 2.2f, transform.position.z);
        Quaternion targetRotation = Quaternion.Euler(25f, transform.eulerAngles.y, 0f);

        float duration = 1f;
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    public IEnumerator CameraDown()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, 1.5f, transform.position.z);
        Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        float duration = 1f;
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}
