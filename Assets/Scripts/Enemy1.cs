using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public Transform player;
    private bool touchingPlayer = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange && !touchingPlayer)
        {
            // Calculate direction
            Vector2 direction = (player.position - transform.position).normalized;
            
            // Move toward the player
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Flip logic: Check if moving right or left
            HandleSpriteFlip(direction.x);
        }
    }

    private void HandleSpriteFlip(float horizontalDirection)
    {
        // If moving right (pos) and scale is left, or moving left (neg) and scale is right
        if (horizontalDirection > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalDirection < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) touchingPlayer = true;
    }

    public void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) touchingPlayer = false;
    }
}