using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float doubleJumpForce = 4.5f;
    [SerializeField] private LayerMask groundLayer = 1; // Default to layer 0 (Default)
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.08f;

    [Header("Attack Settings")]
    [SerializeField] private float attack1Duration = 0.58f; // 7 frames at 12 fps
    [SerializeField] private float attack2Duration = 0.41f; // 5 frames at 12 fps
    [SerializeField] private bool freezeMovementDuringGroundAttack = true;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    
    private bool isGrounded;
    private int jumpsRemaining;

    private bool isAttacking;
    private float attackTimer;

    // Hash IDs for animator parameters (improved performance)
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int Attack1TriggerHash = Animator.StringToHash("Attack1Trigger");
    private static readonly int Attack2TriggerHash = Animator.StringToHash("Attack2Trigger");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Automatically find or create GroundCheck to make setup robust and user-friendly
        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
            if (groundCheck == null)
            {
                GameObject gcObj = new GameObject("GroundCheck");
                gcObj.transform.SetParent(transform);
                
                var col = GetComponent<BoxCollider2D>();
                if (col != null)
                {
                    gcObj.transform.localPosition = new Vector3(col.offset.x, col.offset.y - (col.size.y / 2f), 0f);
                }
                else
                {
                    gcObj.transform.localPosition = new Vector3(0.02f, -0.17f, 0f); // Default estimation
                }
                groundCheck = gcObj.transform;
            }
        }
    }

    private void Update()
    {
        // 1. Check if the player is touching the ground and update jumps
        CheckGroundStatus();

        // Handle attack timer
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }

        // 2. Get horizontal input (Works seamlessly with both Legacy and New Input System)
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            bool leftPressed = keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed;
            bool rightPressed = keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed;

            if (leftPressed && !rightPressed)
                horizontalInput = -1f;
            else if (rightPressed && !leftPressed)
                horizontalInput = 1f;
            else
                horizontalInput = 0f;
        }
        else
        {
            horizontalInput = 0f;
        }
#elif ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            bool leftPressed = keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed;
            bool rightPressed = keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed;

            if (leftPressed && !rightPressed)
                horizontalInput = -1f;
            else if (rightPressed && !leftPressed)
                horizontalInput = 1f;
            else
                horizontalInput = 0f;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
#else
        horizontalInput = Input.GetAxisRaw("Horizontal");
#endif

        // 3. Jump and Double Jump input detection (Cannot jump while attacking)
        bool jumpPressed = false;
        if (!isAttacking)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            if (UnityEngine.InputSystem.Keyboard.current != null)
            {
                jumpPressed = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
            }
#elif ENABLE_INPUT_SYSTEM
            if (UnityEngine.InputSystem.Keyboard.current != null)
            {
                jumpPressed = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
            }
            else
            {
                jumpPressed = Input.GetKeyDown(KeyCode.Space);
            }
#else
            jumpPressed = Input.GetKeyDown(KeyCode.Space);
#endif
        }

        if (jumpPressed && jumpsRemaining > 0)
        {
            ExecuteJump();
        }

        // 4. Attack input detection
        bool attack1Pressed = false;
        bool attack2Pressed = false;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            attack1Pressed = keyboard.jKey.wasPressedThisFrame || keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame;
            attack2Pressed = keyboard.kKey.wasPressedThisFrame || keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame;
        }
#elif ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            attack1Pressed = keyboard.jKey.wasPressedThisFrame || keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame;
            attack2Pressed = keyboard.kKey.wasPressedThisFrame || keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame;
        }
        else
        {
            attack1Pressed = Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1);
            attack2Pressed = Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2);
        }
#else
        attack1Pressed = Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1);
        attack2Pressed = Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2);
#endif

        if (!isAttacking) // Can only trigger attacks if not currently attacking
        {
            if (attack1Pressed)
            {
                ExecuteAttack(1);
            }
            else if (attack2Pressed)
            {
                ExecuteAttack(2);
            }
        }

        // 5. Flip sprite based on direction (Cannot flip mid-attack)
        if (!isAttacking)
        {
            if (horizontalInput > 0f)
            {
                spriteRenderer.flipX = false; // Facing right
            }
            else if (horizontalInput < 0f)
            {
                spriteRenderer.flipX = true;  // Facing left
            }
        }

        // 6. Update animator parameters
        animator.SetBool(IsWalkingHash, horizontalInput != 0f && !isAttacking);
        animator.SetBool(IsGroundedHash, isGrounded);
    }

    private void FixedUpdate()
    {
        float currentHorizontal = horizontalInput;

        // If attacking on ground, stop horizontal movement to make attacks feel weighty
        if (isAttacking && isGrounded && freezeMovementDuringGroundAttack)
        {
            currentHorizontal = 0f;
        }

        // Move the character horizontally using Rigidbody2D physics
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = new Vector2(currentHorizontal * moveSpeed, rb.linearVelocity.y);
#else
        rb.velocity = new Vector2(currentHorizontal * moveSpeed, rb.velocity.y);
#endif
    }

    private void ExecuteAttack(int type)
    {
        isAttacking = true;
        if (type == 1)
        {
            attackTimer = attack1Duration;
            animator.SetTrigger(Attack1TriggerHash);
        }
        else
        {
            attackTimer = attack2Duration;
            animator.SetTrigger(Attack2TriggerHash);
        }

        // If on ground and freeze is enabled, stop horizontal movement immediately
        if (isGrounded && freezeMovementDuringGroundAttack)
        {
#if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
#else
            rb.velocity = new Vector2(0f, rb.velocity.y);
#endif
        }
    }

    private void CheckGroundStatus()
    {
        isGrounded = false;
        if (groundCheck != null)
        {
            // Use OverlapCircleAll to find all overlapping colliders
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                // If the collider belongs to another GameObject, we are on the ground!
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Only reset jumps if we are grounded and not moving upwards (prevents resetting jumps on the exact frame we jump)
        bool isMovingUp = false;
#if UNITY_6000_0_OR_NEWER
        isMovingUp = rb.linearVelocity.y > 0.01f;
#else
        isMovingUp = rb.velocity.y > 0.01f;
#endif

        if (isGrounded && !isMovingUp)
        {
            jumpsRemaining = 2; // Can jump and double jump
        }
        else
        {
            // If we fall off a ledge without jumping, we can only perform 1 jump (double jump)
            if (jumpsRemaining == 2)
            {
                jumpsRemaining = 1;
            }
        }
    }

    private void ExecuteJump()
    {
        // Pick appropriate jump force
        float force = (jumpsRemaining == 1) ? doubleJumpForce : jumpForce;

        // Reset vertical velocity so that the double jump height is consistent regardless of falling speed
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
#else
        rb.velocity = new Vector2(rb.velocity.x, force);
#endif

        jumpsRemaining--;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
