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

    private void Update()
    {
        Debug.Log(isDrifting);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleDrift();
    }

    void HandleMovement()
    {
        // Accelerate or decelerate the car
        currentSpeed += moveInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Apply forward movement based on current speed
        Vector3 forwardMove = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // Steering logic - this rotates the car smoothly based on forward velocity
        if (currentSpeed > 0.1f || currentSpeed < -0.1f) // Ensure the car is moving
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn * Mathf.Sign(currentSpeed), 0f); // Apply rotation direction based on movement direction
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
        // Simple drift detection based on turning and speed
        if (Mathf.Abs(turnInput) > 0.5f && moveInput > 0.1f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        // Adjust sideways friction when drifting
        if (isDrifting)
        {
            Vector3 forwardVelocity = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);
            Vector3 sidewaysVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);

            rb.linearVelocity = forwardVelocity + sidewaysVelocity * driftFactor; // Reduce sideways velocity (drifting effect)

            // Lower traction when drifting
            rb.linearDamping = traction * 0.5f;
        }
        else
        {
            rb.linearDamping = traction; // Normal traction when not drifting
        }
    }
}
