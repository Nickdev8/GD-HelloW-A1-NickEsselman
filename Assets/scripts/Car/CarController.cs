using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")] public float maxSpeed = 1000f;
    public float acceleration = 20f;
    public float brakingForce = 50f;
    public float turnSpeed = 5f;
    public float driftFactor = 0.95f;
    public float traction = 1f;
    public float hopForce = 5f;  // Added hop force for jump

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
        moveInput = inputVector.y;
        turnInput = inputVector.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Hop();  // Trigger hop when jump is performed
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleDrift();
        // ConstrainHeight();
    }

    void HandleMovement()
    {
        currentSpeed += moveInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        Vector3 forwardMove = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.linearVelocity = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z);

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn * Mathf.Sign(currentSpeed), 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        if (brakeInput)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, brakingForce * Time.fixedDeltaTime);
        }
    }

    void HandleDrift()
    {
        if (Mathf.Abs(turnInput) > 0.5f && moveInput > 0.1f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        if (isDrifting)
        {
            Vector3 forwardVelocity = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);
            Vector3 sidewaysVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);

            rb.linearVelocity = forwardVelocity + sidewaysVelocity * driftFactor;
            rb.linearDamping = traction * 0.5f;
        }
        else
        {
            rb.linearDamping = traction;
        }
    }

    void ConstrainHeight()
    {
        if (rb.position.y < minYPosition)
        {
            Vector3 constrainedPosition = new Vector3(rb.position.x, minYPosition, rb.position.z);
            rb.MovePosition(constrainedPosition);
        }
    }

    void Hop()
    {
        rb.AddForce(Vector3.up * hopForce, ForceMode.Impulse);  // Simple hop
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            Debug.Log(other.gameObject.name + " collided with ground");
            currentSpeed = 0;
        }
    }
}