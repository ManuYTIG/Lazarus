using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public string targetTag = "Player";

    private Transform target;
    private bool touchingTarget = false;
    private float baseScaleX;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
        if (obj != null)
            target = obj.transform;

        baseScaleX = transform.localScale.x; // store original scale
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
        if (Mathf.Abs(horizontalDirection) < 0.01f)
            return; // avoid jitter when moving mostly vertically

        float newScaleX = baseScaleX * (horizontalDirection > 0 ? 1 : -1);
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
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