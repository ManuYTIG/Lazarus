using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isFacingRight = true; // Tracks current direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement = new Vector2(x, y).normalized;

        // Check if we need to flip the sprite based on horizontal input
        if (x > 0 && !isFacingRight) {
            Flip();
        } else if (x < 0 && isFacingRight) {
            Flip();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void Flip()
    {
        // Toggle the state
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1 to mirror it
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}
