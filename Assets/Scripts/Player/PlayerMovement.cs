using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public float fallSpeed = -10f;
    public float gravity = -9.81f;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                return IsMoving ? moveSpeed : 0;
            }
            else
            {
                return 0;
            }
        }
        set
        {

        }
    }

    public bool isGrounded;

    private bool isMoving;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        private set
        {
            isMoving = value;
            anim.SetBool(AnimStrings.isMoving, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimStrings.canMove);
        }
    }

    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip land;

    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector3 velocity;

    private CharacterController controller;
    private Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        velocity.x = controller.velocity.x;

        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        // Gravity application
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }
        else if (!isGrounded)
        {
            velocity.y = Mathf.Lerp(velocity.y, fallSpeed, 1.5f * Time.deltaTime);
        }

        anim.SetFloat(AnimStrings.yVel, velocity.y);
        anim.SetBool(AnimStrings.isGrounded, isGrounded);

        Vector3 moveDirection = new (movement.x, 0f, movement.y); ;
        if(CanMove)
            controller.Move(moveSpeed * Time.deltaTime * moveDirection);

        if (IsMoving)
        {
            SetFacingDirection(movement);
        }

        // Apply gravity
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (CanMove)
        {
            movement = context.ReadValue<Vector2>();
            IsMoving = movement != Vector2.zero;
        }

    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        // Calculate the angle in degrees between the current forward direction and the desired move direction.
        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg - 90f;

        // Create a target rotation based on the angle.
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        // Smoothly interpolate between the current rotation and the target rotation.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove)
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(-2f * jumpForce * gravity);
                anim.SetTrigger(AnimStrings.jumpTrig);
                Debug.Log("Jump!");
            }
        }
    }

    public void PlayWalkOne()
    {
        audioSource.PlayOneShot(walk1, 0.45f);
    }

    public void PlayWalkTwo()
    {
        audioSource.PlayOneShot(walk2, 0.45f);
    }

    public void Land()
    {
        audioSource.PlayOneShot(land, 0.15f);
    }
}
