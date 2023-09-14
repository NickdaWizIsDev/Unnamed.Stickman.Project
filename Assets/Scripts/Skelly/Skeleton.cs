using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public float detectionRange;
    public float moveSpeed = 2.0f; // Adjust this to control the speed.
    public float minDistance = 0.1f; // Adjust this to control how close they need to be.
    public float knockbackForce;
    public float upwardForce;
    public float attackTimer = 5f;

    public bool foundPlayer = false;

    public Transform playerTransform;
    public Transform bow;
    public GameObject player;
    public GameObject arrowPrefab;
    public LayerMask playerLayer;

    private GameObject arrow;
    Damageable damageable;
    Animator animator;
    Rigidbody rb;

    private void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        player = GameObject.Find(name: ("Main"));
        playerTransform = player.transform;
    }

    private void Update()
    {
        DetectPlayer();

        if (foundPlayer)
        {
            animator.SetBool(AnimStrings.foundPlayer, foundPlayer);

            if(attackTimer <= 0f)
            {
                Shoot();
                attackTimer = 5f;
            }
            else if (attackTimer > 0f)
            {
                attackTimer-= Time.deltaTime;
            }

            // Calculate the direction from the skeleton to the player.
            Vector3 directionToPlayer = playerTransform.position - transform.position;

            // Calculate the rotation to face the player.
            Quaternion targetRotation = Quaternion.Euler(0, Quaternion.LookRotation(directionToPlayer).eulerAngles.y + 90, 0);

            // Apply the rotation to the skeleton's GameObject.
            transform.rotation = targetRotation;
        }

        if (damageable.IsHit)
        {
            Knockback();
            if(arrow != null)
            {
                Destroy(arrow);
            }
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

    public void Knockback()
    {
        // Calculate the direction from the skeleton to the player.
        Vector3 knockbackDirection = (transform.position - playerTransform.position).normalized;

        // Calculate the knockback velocity.
        Vector3 knockbackVelocity = knockbackDirection * knockbackForce;

        // Apply additional force in the y-axis for a slight upward motion.
        knockbackVelocity.y += upwardForce;

        // Apply the velocity to the Rigidbody.
        rb.velocity = knockbackVelocity;
    }

    public void Shoot()
    {
        animator.SetTrigger(AnimStrings.atkTrig);
        Debug.Log(gameObject.name + " is shooting!");
    }

    public void ArrowLoad()
    {
        Vector3 direction = new Vector3(player.transform.position.x - bow.position.x, 0f, player.transform.position.z - bow.position.z);

        // Calculate the rotation to look in the direction of the shot.
        Quaternion rotation = Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y + 90, 0);
        arrow = Instantiate(arrowPrefab, bow.transform.position, rotation);
    }

    public void ArrowShot()
    {
        Vector3 direction = new Vector3(player.transform.position.x - bow.position.x, 0f, player.transform.position.z - bow.position.z);

        // Calculate the rotation to look in the direction of the shot.
        Quaternion rotation = Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y + 90, 0);

        // Set the arrow's rotation.
        arrow.transform.rotation = rotation;

        // Set the arrow's velocity to shoot it in the calculated direction.
        arrow.GetComponent<Rigidbody>().velocity = direction.normalized * 10f;

        Destroy(arrow, 1f);
    }
}
