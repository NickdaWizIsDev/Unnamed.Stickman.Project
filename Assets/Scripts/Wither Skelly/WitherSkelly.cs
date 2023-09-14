using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitherSkelly : MonoBehaviour
{
    public float detectionRange;
    public float moveSpeed = 5.0f; // Adjust this to control the speed.
    public float minDistance = 1f; // Adjust this to control how close they need to be.
    public float knockbackForce;

    public float attackTimer = 0f;

    public bool foundPlayer = false;

    public bool ready = false;

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

        if (damageable.IsHit)
        {
            Knockback();
        }
        if (!ready)
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer >= 2f)
        {
            ready = true;
        }
    }

    private void FixedUpdate()
    {
        if (foundPlayer && !damageable.IsHit)
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
                Debug.Log(gameObject.name + "Found player!");
                foundPlayer = true;
            }
        }
    }

    private void Move()
    {
        bool isMoving = animator.GetBool(AnimStrings.isMoving);

        if (isMoving)
        {
            // Calculate the direction to the player.
            Vector3 directionToPlayer = playerTransform.position - transform.position;

            // Normalize the direction vector to get a unit vector.
            directionToPlayer.Normalize();

            // Calculate the velocity to move towards the player.
            Vector3 moveVelocity = new(directionToPlayer.x * moveSpeed, rb.velocity.y, directionToPlayer.z * moveSpeed);

            // Apply the velocity to the Rigidbody.
            rb.velocity = moveVelocity;

            // Smoothly interpolate the rotation.
            float rotationSpeed = 5.0f; // Adjust this to control the rotation speed.
            Quaternion targetRotation = Quaternion.Euler(0, Quaternion.LookRotation(directionToPlayer).eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the distance between the Zombie and player is very close.
            if(Vector3.Distance(transform.position, playerTransform.position) < minDistance)
            {
                rb.velocity = Vector3.zero;
            }

            if (ready && Vector3.Distance(transform.position, playerTransform.position) < minDistance)
            {
                ready = false;
                attackTimer = 0f;
                // Optionally, you can stop moving or trigger an attack animation.
                animator.SetTrigger(AnimStrings.atkTrig);
            }
        }
    }

    public void Knockback()
    {
        // Calculate the direction from the skeleton to the player.
        Vector3 knockbackDirection = (transform.position - playerTransform.position).normalized;

        // Calculate the knockback velocity.
        Vector3 knockbackVelocity = knockbackDirection * knockbackForce;

        // Apply the velocity to the Rigidbody.
        rb.velocity = knockbackVelocity;
    }
}
