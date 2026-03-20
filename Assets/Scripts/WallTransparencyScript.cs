using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTransparencyScript : MonoBehaviour
{
    private Transform player;
    public float fadedAlpha = 0.3f;
    public float normalAlpha = 1f;
    public float fadeSpeed = 5f;

    Tilemap tilemap;
    TilemapRenderer tilemapRenderer;
    SpriteRenderer spriteRenderer;

    float targetAlpha;

    void Start()
    {   
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Try to get both components (only one will exist)
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool inside = false;

        // --- TILEMAP MODE ---
        if (tilemap != null)
        {
            Vector3Int cell = tilemap.WorldToCell(player.position);
            inside = tilemap.HasTile(cell);
        }

        // --- SPRITE MODE ---
        if (spriteRenderer != null)
        {
            // Check if player is overlapping the sprite's bounds
            inside = spriteRenderer.bounds.Contains(player.position);
        }

        // Set fade target
        targetAlpha = inside ? fadedAlpha : normalAlpha;

        // Apply fade to TilemapRenderer
        if (tilemapRenderer != null)
        {
            Color c = tilemapRenderer.material.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            tilemapRenderer.material.color = c;
        }

        // Apply fade to SpriteRenderer
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            spriteRenderer.color = c;
        }
    }
}