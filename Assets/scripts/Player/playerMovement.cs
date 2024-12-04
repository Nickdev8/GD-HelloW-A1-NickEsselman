using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the camera's Transform
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    
    private Vector2 moveInput = Vector2.zero;
    private bool jumping = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this GameObject.");
        }
        
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned.");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Forward/Backward, Left/Right
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumping = true;
        if (context.canceled)
            jumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate movement direction based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten the forward and right vectors on the y-axis to prevent unwanted vertical movement
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate the direction relative to the camera's orientation
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x) * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);

        // Check for jump and apply force
        if (jumping && Mathf.Approximately(rb.linearVelocity.y, 0)) // Jump if grounded
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumping = false; // Reset jump flag after jumping
        }
    }
}
