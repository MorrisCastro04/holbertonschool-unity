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
    private Rigidbody rb;
    private bool controlLateralActivo = false;

    private InputAction moveLateralAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("La bola de bowling necesita un Rigidbody.");
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canal"))
        {
            SetControlLateralActivo(false);
        }
    }
}