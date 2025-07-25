using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    public InputActionAsset bowlingControlsAsset;
    public float velocidadLateral = 5f;

    public float speedMultiplier = 2f;
    private Rigidbody rb;
    private bool controlLateralActivo = false;

    bool speedMultiplierActive = false;

    private InputAction moveLateralAction;

    ObstacleSystem obstacleSystem;

    CameraController cameraController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("La bola de bowling necesita un Rigidbody.");
            enabled = false;
            return;
        }

        cameraController = GameObject.Find("Main Camera")?.GetComponent<CameraController>();
        if (!cameraController)
        {
            Debug.LogError("No se encontró el CameraController en la cámara principal.");
            enabled = false;
            return;
        }

        obstacleSystem = GameObject.Find("obstacleSystem")?.GetComponent<ObstacleSystem>();
        if (!obstacleSystem)
        {
            Debug.LogError("No se encontró el ObstacleSystem en la escena.");
            enabled = false;
            return;
        }

        moveLateralAction = bowlingControlsAsset.FindActionMap("BallControls").FindAction("MoveLateralVR");

        if (moveLateralAction == null)
        {
            Debug.LogError("No se encontró la acción 'MoveLateral' en el Input Action Asset.");
            enabled = false;
        }
    }

    void OnEnable()
    {
        if (moveLateralAction != null)
        {
            moveLateralAction.Enable();
        }
    }

    void OnDisable()
    {
        if (moveLateralAction != null)
        {
            moveLateralAction.Disable();
        }
    }
    void FixedUpdate()
    {
        if (controlLateralActivo)
        {
            Vector2 movimientoLateral = moveLateralAction.ReadValue<Vector2>();

            if (speedMultiplierActive)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * speedMultiplier);
                speedMultiplierActive = false;
            }
            Vector3 fuerzaLateral = transform.right * movimientoLateral * velocidadLateral;

            rb.AddForce(fuerzaLateral, ForceMode.Force);
    }
}


    void SetControlLateralActivo(bool activo)
    {
        controlLateralActivo = activo;
        if (!activo)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canal"))
        {
            SetControlLateralActivo(true);
            cameraController.StartCoroutine(cameraController.CameraUP());
            obstacleSystem.SpawnObstacle();
        }

        if (other.CompareTag("SpeedPad"))
        {
            speedMultiplierActive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canal"))
        {
            SetControlLateralActivo(false);
            cameraController.StartCoroutine(cameraController.CameraDown());
            speedMultiplierActive = false;
            obstacleSystem.ClearObstacles();
        }
    }
}