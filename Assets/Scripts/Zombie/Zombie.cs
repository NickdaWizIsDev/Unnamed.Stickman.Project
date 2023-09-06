using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float detectionRange;
    public float moveSpeed = 2.0f; // Adjust this to control the speed.
    public float minDistance = 0.1f; // Adjust this to control how close they need to be.

    public bool foundPlayer = false;

    public Transform playerTransform;
    public GameObject player;
    public LayerMask playerLayer;
    Damageable damageable;
    Animator animator;
    Rigidbody rb;

    private void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    private void Update()
    {
        DetectPlayer();

        animator.SetBool(AnimStrings.foundPlayer, foundPlayer);        
    }

    private void FixedUpdate()
    {
        if (foundPlayer)
        {
            Move();
        }
    }

    private void DetectPlayer()
    {
        Vector3 playerPosition = playerTransform.position;

        // Calculate the direction to the player.
        Vector3 distanceToPlayer = playerPosition - transform.position;

        // Check if the player is within the detection range.
        if (distanceToPlayer.x <= detectionRange & !foundPlayer)
        {
            if (Physics.CheckSphere(transform.position, detectionRange, playerLayer))
            {
                // Player detected within the sphere.
                // Implement your logic here.
                Debug.Log("Found player!");
                foundPlayer = true;
            }
        }
    }

    private void Move()
    {
        bool isMoving = animator.GetBool(AnimStrings.isMoving);

        if (isMoving)
        {
            // Calculate the direction to the player on the x-axis.
            float directionX = playerTransform.position.x > transform.position.x ? 1.0f : -1.0f;

            // Calculate the velocity to move towards the player.
            Vector3 moveVelocity = new Vector3(directionX * moveSpeed, rb.velocity.y, 0.0f);

            // Apply the velocity to the Rigidbody.
            rb.velocity = moveVelocity;

            Quaternion targetRotation;
            if (directionX > 0)
            {
                // Rotate 180 degrees around the y-axis (up).
                targetRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else
            {
                // Rotate back to 0 degrees.
                targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }

            // Smoothly interpolate the rotation.
            float rotationSpeed = 5.0f; // Adjust this to control the rotation speed.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the distance on the x-axis between the Zombie and player is very close.
            if (Mathf.Abs(playerTransform.position.x - transform.position.x) < minDistance)
            {
                // Optionally, you can stop moving or trigger an attack animation.
                // Match the Zombie's position with the player's exact position.
                transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
            }
        }
    }


}
