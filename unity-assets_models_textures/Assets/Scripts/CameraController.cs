using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 100;
    Vector3 offset;


    void Awake()
    {
        offset = player.position - transform.position;
    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
        offset = rotation * offset;
        transform.position = player.position - offset;
        transform.LookAt(player);
    }
}
