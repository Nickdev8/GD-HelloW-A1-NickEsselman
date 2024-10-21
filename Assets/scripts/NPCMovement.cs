using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public List<Transform> waypoints; // List of waypoints to move towards
    public float speed = 2f;          // Speed of NPC movement
    public float stoppingDistance = 0.5f; // Distance to stop moving when reaching the waypoint
    public float rotationSpeed = 5f;  // Speed of NPC rotation

    public GameObject armoture;
    public Animator animator; // Reference to the Animator component

    private int currentWaypointIndex = 0; // Current waypoint index
    private bool isRagdoll = false; // Track if the NPC is in ragdoll mode

    void Start()
    {
        SetRagdollState(false); // Start with ragdoll disabled
    }

    void Update()
    {
        // Check if the NPC is in ragdoll mode
        if (isRagdoll) return;

        // Check if there are any waypoints
        if (waypoints.Count == 0) return;

        // Move towards the current waypoint
        MoveToCurrentWaypoint();
    }

    // Move NPC towards the current waypoint
    private void MoveToCurrentWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        float step = speed * Time.deltaTime; // Calculate distance to move

        // Move towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Rotate to face the waypoint
        Vector3 targetDirection = (targetWaypoint.position - transform.position).normalized; // Direction to the waypoint
        if (targetDirection != Vector3.zero) // Prevent NaN errors when the direction is zero
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Create rotation to face the waypoint
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Smoothly rotate towards the target
        }

        // Check if the NPC has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < stoppingDistance)
        {
            // If reached, update to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; // Loop back to the start if at the end
        }
    }

    // Enable or disable ragdoll mode
    private void SetRagdollState(bool state)
    {
        isRagdoll = state;

        // Enable or disable the Animator
        //animator.enabled = !state;

        // If entering ragdoll mode, add colliders and rigidbodies
        if (state)
        {
            AddCollidersAndRigidbodies(armoture.transform);
        }
    }

    // Adds colliders and rigidbodies to all children of the armature
    private void AddCollidersAndRigidbodies(Transform parent)
    {
        // Iterate through all children of the NPC
        foreach (Transform child in parent)
        {
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            
            if(rb != null)
                rb.isKinematic = false;

            if (child.transform.childCount > 0)
                AddCollidersAndRigidbodies(child);
        }
    }

    // Called when the player enters the NPC's trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isRagdoll)
        {
            SetRagdollState(true); // Activate ragdoll mode
        }
    }
}
