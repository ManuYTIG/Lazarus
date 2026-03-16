using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public Transform player;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            // Move toward the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }
}