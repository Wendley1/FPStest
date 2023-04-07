using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private readonly float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    private readonly float movementMultiplier = 10f;

    public bool Moving { get; private set; }

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    public bool Sprinting { get; private set; }

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [Space(5)]
    [SerializeField] private Transform arms;
    [Space(5)]
    [SerializeField] private float armsLandingSmooth;
    [SerializeField] private float armsJumpingSmooth;
    [Space(5)]
    [SerializeField] private float normalY;
    [SerializeField] private float maxYDown;
    [SerializeField] private float maxYUp;

    private float y;
    private float armsSmooth;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.15f;

    public bool IsGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    float t = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && IsGrounded)
        {
            Jump();
            t = 0.2f;
        }
        else if (!IsGrounded)
        {
            y += Time.deltaTime * 0.15f;

            if(y > maxYUp)
                y = maxYUp;

            armsSmooth = armsJumpingSmooth;
        }
        else 
        {
            if(t < 0) 
            {
                y = normalY;
                armsSmooth = armsLandingSmooth;
            }
            else
            {
                y = maxYDown;
                armsSmooth = armsJumpingSmooth * 2;
                t -= Time.deltaTime;
            }
        }

        Vector3 mov = new(0, y, 0);

        arms.localPosition = Vector3.Lerp(arms.localPosition, mov, Time.deltaTime * armsSmooth);

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        Moving = horizontalMovement != 0 || verticalMovement != 0;
    }

    void Jump()
    {
        if (IsGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        Sprinting = Input.GetKey(sprintKey) && Input.GetKey(KeyCode.W);

        moveSpeed = Sprinting && IsGrounded ?
            Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime) :
            Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    }

    void ControlDrag()
    {
        rb.drag = IsGrounded ?
            groundDrag :
            airDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (IsGrounded && !OnSlope())
        {
            rb.AddForce(movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
        }
        else if (IsGrounded && OnSlope())
        {
            rb.AddForce(movementMultiplier * moveSpeed * slopeMoveDirection.normalized, ForceMode.Acceleration);
        }
        else if (!IsGrounded)
        {
            rb.AddForce(airMultiplier * movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
        }
    }
}