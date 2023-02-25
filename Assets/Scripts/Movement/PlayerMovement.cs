using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerPrefab;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;


    [Header("Jump Controls")] 
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool canJump;

    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Keybinds")] 
    public KeyCode JumpKey = KeyCode.Space;

    private float horizontalInput;
    private float verticalInput;
    
    private Vector3 moveDirection;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(playerPrefab.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MovementInput();
        SpeedControl();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed += 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed -= 2;
        }

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(JumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = playerPrefab.forward * verticalInput + playerPrefab.right * horizontalInput;
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}
