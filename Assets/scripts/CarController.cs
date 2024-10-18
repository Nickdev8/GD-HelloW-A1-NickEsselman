using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxSpeed = 1000f;
    public float acceleration = 20f;
    public float brakingForce = 50f;
    public float turnSpeed = 5f;
    public float driftFactor = 0.95f;
    public float traction = 1f;
    
    // Minimum Y position for the car (ground level)
    private float minYPosition = -12.0164f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool isDrifting = false;

    private float moveInput = 0f;
    private float turnInput = 0f;
    private bool brakeInput = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveInput = inputVector.y;  // Forward/Backward
        turnInput = inputVector.x;  // Left/Right
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            brakeInput = true;
        if (context.canceled)
            brakeInput = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleDrift();
        ConstrainHeight();
    }

    void HandleMovement()
    {
        // Accelerate or decelerate the car based on input
        currentSpeed += moveInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Apply forward movement
        Vector3 forwardMove = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.linearVelocity = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z); // Preserve vertical velocity

        // Steering logic - only turn when the car is moving
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn * Mathf.Sign(currentSpeed), 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // Handle braking
        if (brakeInput)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, brakingForce * Time.fixedDeltaTime);
        }
    }

    void HandleDrift()
    {
        // Detect drift based on turning and forward movement
        if (Mathf.Abs(turnInput) > 0.5f && moveInput > 0.1f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        // Adjust velocity and traction if drifting
        if (isDrifting)
        {
            Vector3 forwardVelocity = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);
            Vector3 sidewaysVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);

            rb.linearVelocity = forwardVelocity + sidewaysVelocity * driftFactor;

            rb.linearDamping = traction * 0.5f; // Reduce traction while drifting
        }
        else
        {
            rb.linearDamping = traction; // Reset to normal traction
        }
    }

    void ConstrainHeight()
    {
        // Ensure the car does not go below the minimum Y position (ground level)
        if (rb.position.y < minYPosition)
        {
            Vector3 constrainedPosition = new Vector3(rb.position.x, minYPosition, rb.position.z);
            rb.MovePosition(constrainedPosition); // Snap the car back to the minimum height
        }
    }
}
