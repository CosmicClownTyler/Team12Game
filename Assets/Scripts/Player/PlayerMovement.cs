using UnityEngine;

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
    private bool jumping;
    private Rigidbody rb;

    // Draw player height
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * (playerHeight * 0.5f + 0.1f)));
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;
    }

    private void Update()
    {
        // get input
        moveAmount = InputManager.Instance.MoveInput;
        jumping = InputManager.Instance.JumpWasPressed;

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);
    }

    private void FixedUpdate()
    {
        Move();
        SpeedHandler();
        
        if (canJump && grounded && jumping)
        {
            Jump();
        }

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

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
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(JumpRestart), jumpCooldown);
    }
    private void JumpRestart()
    {
        canJump = true;
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
    }
}