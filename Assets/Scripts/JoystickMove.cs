using UnityEngine;

public class JoystickMove : MonoBehaviour {

    public float playerSpeed;
    
    public Joystick movementJoystick;
    public Animator animator;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // player moving joystick
        if (movementJoystick.Direction.magnitude > 0)
        {
            Vector2 direction = new Vector2(movementJoystick.Direction.x * playerSpeed, movementJoystick.Direction.y * playerSpeed);
            rb.linearVelocity = direction;
            // let animator know that player is moving with current direction
            AnimateMovement(direction);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            // let animator know that player is no longer moving
            AnimateMovement(Vector2.zero);
        }
    }

    // Updates player animator controller
    void AnimateMovement(Vector2 direction)
    {
        if (animator != null)
        {
            //animator.SetBool("Hoeing", false);
            if (direction.magnitude > 0)
            {
                // transitions animation to running state
                animator.SetBool("isMoving", true);
                // updates animation based on direction
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
            else
            {
                // transitions animation to idle state
                animator.SetBool("isMoving", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Collided with: " + other.gameObject.name);

    }
}
