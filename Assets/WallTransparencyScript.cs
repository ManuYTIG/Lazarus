using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTransparencyScript : MonoBehaviour
{
    public Transform player;
    public TilemapRenderer tilemapRenderer;
    public float fadedAlpha = 0.3f;
    public float normalAlpha = 1f;
    public float fadeSpeed = 5f;

    Tilemap tilemap;
    float targetAlpha;

    void Start()
    {
        tilemap = tilemapRenderer.GetComponent<Tilemap>();
    }

    void Update()
    {
        // Convert player world position to the tilemap cell
        Vector3Int cell = tilemap.WorldToCell(player.position);

        // Check if the player is inside a tile
        bool insideTile = tilemap.HasTile(cell);

        // Set fade target
        targetAlpha = insideTile ? fadedAlpha : normalAlpha;

        // Smooth fade
        Color c = tilemapRenderer.material.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        tilemapRenderer.material.color = c;
    }
}