using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public LayerMask groundMask;
    public float groundDistance;
    public float maxSlopeAngle;
    bool isGrounded;
    CapsuleCollider capsule;
    RaycastHit slopeHit, groundHit;
    bool onSlope;
    bool exitingSlope;
    

    public Transform orientation;

    float hor;
    float ver;

    Vector3 moveDir;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        GroundCheck();

        GrabInput();
        SpeedControl();
        StateHandler();

        // handle drag
        rb.drag = (isGrounded) ? groundDrag : 0f;
    }

    private void GroundCheck()
    {
        float height = capsule.height / 2f + groundDistance - capsule.radius;
        isGrounded = Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out groundHit, height, groundMask);

        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, capsule.height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            onSlope = angle < maxSlopeAngle && angle != 0;
        }else {
            onSlope = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GrabInput()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetButton("Jump") && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void StateHandler()
    {
        if(isGrounded && Input.GetButton("Sprint") && ver > 0f)
        {
            moveSpeed = sprintSpeed;
        }else if (isGrounded)
        {
            moveSpeed = walkSpeed;
        }

    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDir = orientation.forward * ver + orientation.right * hor;
        moveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;

        // on slope
        if (onSlope && !exitingSlope)
        {
            rb.AddForce(moveDir * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        else if(isGrounded)
            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);

        else if(!isGrounded)
            rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !isGrounded;
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (onSlope && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
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

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }
}
