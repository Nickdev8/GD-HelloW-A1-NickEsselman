using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        moveInput = context.ReadValue<Vector2>().y;
        turnInput = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            brakeInput = true;
        else if (context.canceled)
            brakeInput = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Debug.Log(moveInput);
        //Debug.Log(turnInput);
        //Debug.Log(brakeInput);

        HandleMovement(moveInput, turnInput, brakeInput);
        HandleDrift(moveInput, turnInput);
    }

    void HandleMovement(float moveInput, float turnInput, bool brakeInput)
    {
        // Accelerate or decelerate the car
        currentSpeed += moveInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Apply speed to Rigidbody
        Vector3 forwardMove = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // Handle braking
        if (brakeInput)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, brakingForce * Time.fixedDeltaTime);
        }

        // Turning
        float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void HandleDrift(float moveInput, float turnInput)
    {
        // Simple drift detection (based on player inputs or velocity angle)
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
            rb.linearVelocity = forwardVelocity + sidewaysVelocity * driftFactor;

            // Lower the traction when drifting
            rb.linearDamping = traction * 0.5f; 
        }
        else
        {
            rb.linearDamping = traction;
        }
    }
}
