using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public float fallSpeed = -10f;
    public float gravity = -9.81f;

    private CharacterController controller;
    [SerializeField]private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Check if the player is grounded
        bool isGrounded = controller.isGrounded;

        // Gravity application
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else if (!isGrounded && velocity.y < 0)
        {
            velocity.y = Mathf.Lerp(velocity.y, fallSpeed, 2f * Time.deltaTime);
        }

        // Movement input and application
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = transform.right * moveInput;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Jump input and application
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Calculate the initial jump velocity based on jumpForce and gravity
            velocity.y = Mathf.Sqrt(-2f * jumpForce * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
