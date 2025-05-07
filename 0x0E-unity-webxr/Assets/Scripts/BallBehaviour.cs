using System.Collections; // Required for IEnumerator
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 3.0f; // Forward movement speed
    public float horizontalSpeed = 2f; // Horizontal movement speed (left/right)
    private bool isOnPiste = false; // Whether the ball is on the piste
    public static bool disableMouseKeyboard = false; // Disables player input when true
    private bool trapSpawned = false;
    public SpawnTrap spawn;

    private float originalSpeed; // Stores the base speed of the ball

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed; // Initialize the base speed
    }

    private void Update()
    {
        if (isOnPiste)
        {
            HandleBallControl(); // Handle ball movement when on the piste
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the ball enters the piste
        if (other.CompareTag("pist"))
        {
            Debug.Log("On the piste");
            isOnPiste = true;
            disableMouseKeyboard = true; // Disable player input

            if(!trapSpawned && spawn != null)
            {
                spawn.Spawn();
                trapSpawned = true;
            }
        }

        // Check if the ball enters a "speed" zone
        if (other.CompareTag("speed"))
        {
            StartCoroutine(TemporarySpeedBoost()); // Trigger the speed boost effect
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the ball exits the piste
        if (other.CompareTag("pist"))
        {
            isOnPiste = false;
            disableMouseKeyboard = false; // Re-enable player input
        }
    }

    private void HandleBallControl()
    {
        // Get horizontal input for left/right movement
        float moveX = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;

        // Current position of the ball
        Vector3 currentPosition = rb.position;

        // Calculate new movement: always move forward and adjust horizontal position
        Vector3 movement = new Vector3(moveX, 0, speed * Time.deltaTime);

        // Update the ball's position
        rb.MovePosition(currentPosition + movement);
    }

    private IEnumerator TemporarySpeedBoost()
    {
        // Double the speed
        speed = originalSpeed * 2;

        // Wait for 1 second
        yield return new WaitForSeconds(1);

        // Gradually reduce speed back to the original value
        while (speed > originalSpeed)
        {
            speed -= Time.deltaTime * originalSpeed; // Adjust the decrement factor as needed
            yield return null; // Wait until the next frame
        }

        // Ensure the speed resets exactly to the original value
        speed = originalSpeed;
    }
}
