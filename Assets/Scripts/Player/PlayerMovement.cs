using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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

    private bool isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        private set
        {
            if (isFacingRight != value)
            {
                // Calculate the new rotation based on facing direction
                Vector3 newScale = transform.localScale ;
                newScale.x *= -1f;

                // Apply the new rotation to the character's transform
                transform.localScale = newScale;
            }

            isFacingRight = value;
        }
    }

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

    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector3 velocity;

    private CharacterController controller;
    private Animator anim;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();        
    }

    private void Update()
    {
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

        Vector3 moveDirection = transform.right * movement;
        controller.Move(moveSpeed * Time.deltaTime * moveDirection);

        // Apply gravity
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        IsMoving = movement != Vector2.zero;

        SetFacingDirection(movement);
    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(-2f * jumpForce * gravity);
                anim.SetTrigger(AnimStrings.jumpTrig);
                Debug.Log("Jump!");
            }
        }
    }
}
