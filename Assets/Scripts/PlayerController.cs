using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Physics
    Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public float gravity;
    // Ground
    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    // Input
    PlayerInputActions playerInput;
    float inputX;
    // Animation
    Animator anim;

    void Awake() {
        // Get/Create components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = new PlayerInputActions();
        // Input
        playerInput.Player.Jump.performed += context => Jump();
        playerInput.Player.Jump.canceled += context => StopJump();
        playerInput.Player.Movement.performed += context => Movement(context.ReadValue<float>());
        playerInput.Player.Movement.canceled += context => StopMovement();
    }

    void OnEnable() {
        playerInput.Enable();
    }

    void Update() {
        // Is player grounded?
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        // Is player running?
        if (inputX == 0) {
            anim.SetBool("isRunning", false);
        } else {
            anim.SetBool("isRunning", true);
        }
        // Is player jumping?
        if (isGrounded == false) {
            anim.SetBool("isJumping", true);
        } else {
            anim.SetBool("isJumping", false);
        }
        // Flip sprite
        if (inputX > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (inputX < 0) {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    
    void FixedUpdate() {
        // Movement
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
    }

    void OnDisable() {
        playerInput.Disable();
    }
    
    // Custom methods
    void Jump() {
        if (isGrounded == true) {
            anim.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    void StopJump() {
        rb.velocity = new Vector2(rb.velocity.x, gravity);
    }

    void Movement(float direction) {
        inputX = direction;
    }

    void StopMovement() {
        inputX = 0;
    }
}
