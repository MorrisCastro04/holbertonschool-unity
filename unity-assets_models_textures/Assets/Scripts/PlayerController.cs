using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 10;
    public LayerMask groundLayer;
    public bool onGround;
    float gravity = 9.8f;
    Vector2 _move;
    Vector3 move;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * -0.4f), 0.7f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        onGround = Physics.CheckSphere(transform.position + (Vector3.up * -0.4f), 0.7f, groundLayer);
        if (!onGround)
            rb.AddForce(Vector3.up * -gravity, ForceMode.Acceleration);

        if (_move.x != 0 || _move.y != 0)
        {
            move = new Vector3(_move.x, 0, _move.y);
            rb.velocity = move * speed + new Vector3(0, rb.velocity.y, 0);
        }
    }
    void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
        if (_move.x == 0 || _move.y == 0)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    void OnJump()
    {
        if (onGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
