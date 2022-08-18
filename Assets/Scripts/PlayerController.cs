using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Physics
    Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public float gravity;
    bool isFalling; // Float fix
    // Ground
    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    // Controls
    float inputX;
    // Animation
    Animator anim;

    void Awake() {
        rb = GetComponent<Rigidbody2D>(); // Set rb to Player's Rigidbody2D
        anim = GetComponent<Animator>(); // Set anim to Player's Animator
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround); // Check if Player is grounded

        // Check if player is running (for animation)
        if (inputX == 0) {
            anim.SetBool("isRunning", false);
        } else {
            anim.SetBool("isRunning", true);
        }

        // Check if player is jumping (for animation)
        if (isGrounded == false) {
            anim.SetBool("isJumping", true);
        } else {
            anim.SetBool("isJumping", false);
        }
        
        // Flip sprite when facing left
        if (inputX > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (inputX < 0) {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    
    void FixedUpdate() {
        // Check if Player is falling
        if (rb.velocity.y < 0) {
            isFalling = true;
        } else if (rb.velocity.y >= 0) {
            isFalling = false;
        }
        
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y); // Horizontal movement
    }
    
    // Custom methods
    public void Jump(InputAction.CallbackContext context) {
        // Jump
        if (context.performed == true && isGrounded == true) {
            anim.SetTrigger("takeOff"); // Trigger jump animation
            rb.velocity = Vector2.up * jumpForce;
        } else if (context.canceled == true && isFalling == false) {
            rb.velocity = new Vector2(rb.velocity.x, gravity);
        }
    }

    public void Movement(InputAction.CallbackContext context) {
        // Horizontal movement
        inputX = context.ReadValue<Vector2>().x;
    }
}
