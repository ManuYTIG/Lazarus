using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BreakableBox box; // Reference to the breakable box that the player can interact with
    public GameObject bullet; // Reference to the bullet prefab for the plasma pistol
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isFacingRight = true;
    public InventorySystem inventorySystem; // Reference to the player's inventory system

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement = new Vector2(x, y).normalized;

        if (x > 0 && !isFacingRight) Flip();
        else if (x < 0 && isFacingRight) Flip();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            box.BreakBox();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemData item = inventorySystem.Getitem();

            if (item != null)
            {
                Debug.Log($"Using item: {item.Name}");
                // Implement item usage logic here
                if (item.ID == "pistol_plasma")
                {
                    // Add logic to fire the plasma pistol, such as instantiating a projectile
                    Vector3 bulletSpawnPos = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.5f;
                    GameObject b = Instantiate(bullet, bulletSpawnPos, Quaternion.identity);
                    Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    b.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(isFacingRight ? 10f : -10f, 0); // Example velocity for the bullet
                    Debug.Log($"Fired plasma bullet with {b.GetComponent<Rigidbody2D>().linearVelocity} velocity");
                }
            }
            else
            {
                Debug.Log("No item in inventory to use.");
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}