using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public string targetTag = "Player";

    private Transform target;
    private bool touchingTarget = false;

    void Start()
    {
        // Find the first object with the tag
        GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
        if (obj != null)
            target = obj.transform;
    }

    void Update()
    {
        if (target == null)
        {
            // Try to find again in case the target spawned later
            GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
            if (obj != null)
                target = obj.transform;

            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < detectionRange && !touchingTarget)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            HandleSpriteFlip(direction.x);
        }
    }

    private void HandleSpriteFlip(float horizontalDirection)
    {
        if (horizontalDirection > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalDirection < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
            touchingTarget = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
            touchingTarget = false;
    }
}