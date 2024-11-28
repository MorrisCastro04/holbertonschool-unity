using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 10;
    public Transform cam;
    public LayerMask groundLayer;
    public bool onGround;
    public AudioSource audioSource;
    public AudioClip grassFootstepSound;
    public AudioClip rockFootstepSound;
    public string currentSurface = "grass";

    float gravity = 9.8f;
    Vector2 _move;
    Vector3 move, start;
    Rigidbody rb;
    Animator anim;
    bool isFalling = false;
    bool blockMove = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        start = transform.position;
        anim = GetComponentInChildren<Animator>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * -0.4f), 0.7f);
    }

    void FixedUpdate()
    {
        onGround = Physics.CheckSphere(transform.position + (Vector3.up * -0.4f), 0.7f, groundLayer);

        if (onGround)
        {
            isFalling = false;
            anim.SetBool("falling", false);
        }

        if (blockMove || isFalling)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Happy Idle") && !isFalling)
            {
                blockMove = false;
            }
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        if (!onGround)
        {
            rb.AddForce(Vector3.up * -gravity, ForceMode.Acceleration);
            audioSource.Stop();
        }

        if (_move.x != 0 || _move.y != 0)
        {
            move = cam.forward * _move.y + cam.right * _move.x;
            move.y = 0;
            move.Normalize();

            Vector3 horizontalVelocity = move * speed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

            anim.SetBool("running", true);

            if (onGround && !audioSource.isPlaying)
            {
                audioSource.clip = currentSurface == "grass" ? grassFootstepSound : rockFootstepSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            anim.SetBool("running", false);
            audioSource.Stop();
        }

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(start.x, start.y * 20, start.z);
            anim.SetBool("falling", true);
            anim.SetBool("jump", true);
            blockMove = true;
            isFalling = true;
        }
    }

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
        if (_move.x == 0 && _move.y == 0)
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("running", false);
            audioSource.Stop();
        }
    }

    public void OnJump()
    {
        if (onGround)
        {
            anim.SetTrigger("jump");
            audioSource.Stop();

            Vector2 moveDir = _move;
            if (moveDir != Vector2.zero)
            {
                Vector3 jumpDir = cam.forward * moveDir.y + cam.right * moveDir.x;
                jumpDir.y = 0;
                jumpDir.Normalize();
                Quaternion targetR = Quaternion.LookRotation(jumpDir);
                transform.rotation = targetR;
                rb.AddForce(transform.forward + Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("grass"))
        {
            currentSurface = "grass";
        }
        else if (collision.gameObject.CompareTag("stone"))
        {
            currentSurface = "rock";
        }
    }
}
