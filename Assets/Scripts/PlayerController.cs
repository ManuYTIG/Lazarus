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
    public Health health;


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

        rb.linearVelocity = movement * moveSpeed;
        if (Input.GetMouseButtonDown(0))
        {
            ItemData item = inventorySystem.Getitem();

            if (item != null)
            {
                Debug.Log($"Using item: {item.Name}");
                // Implement item usage logic here
                if (item.ID == "pistol_plasma")
                {
                    // Add logic to fire the plasma pistol, such as instantiating a projectile
                    Vector3 clickScreen = Input.mousePosition;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(clickScreen);
                    worldPos.z = 0f;
 
                    Vector3 clickDir = (worldPos - transform.position).normalized;
                    Vector3 bulletSpawnPos = transform.position + clickDir * 0.5f;
                    float angle = -Vector2.SignedAngle(clickDir, new Vector2(1, 0));
                    GameObject b = Instantiate(bullet, bulletSpawnPos, Quaternion.Euler(0, 0, angle));
                    Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    b.GetComponent<Rigidbody2D>().linearVelocity = clickDir* 20f; // Example velocity for the bullet
                    Debug.Log($"Fired plasma bullet with {b.GetComponent<Rigidbody2D>().linearVelocity} velocity");
                }
                if (item.ID == "light_item") {
                    
                }
            }
            else
            {
                Debug.Log("No item in inventory to use.");
            }
        }
    }

    void FixedUpdate()
    {
        
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}