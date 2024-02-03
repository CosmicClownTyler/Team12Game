using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool canJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Orientation")]
    public Transform orientation;

    private Vector2 moveAmount;
    private Rigidbody rb;

    //[HideInInspector] public TextMeshProUGUI text_speed;

    private void Start()
    {
        Debug.Log("Starting");
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // handle speed
        SpeedHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    // movement events (subscribed to in inspector)
    public void OnMove(InputAction.CallbackContext context)
    {
        // read move input
        moveAmount = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        // when to jump
        if (canJump && grounded)
        {
            Jump();
        }
    }

    private void SpeedHandler()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        //text_speed.SetText("Velocity: " + flatVel.magnitude);
    }

    // actual movement
    private void Move()
    {
        // calculate movement direction
        Vector3 moveDirection = orientation.forward * moveAmount.y + orientation.right * moveAmount.x;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    private void Jump()
    {
        canJump = false;

        // reset y velocity
        Debug.Log("Jumping");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(JumpRestart), jumpCooldown);
    }
    private void JumpRestart()
    {
        canJump = true;
    }
}