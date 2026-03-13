using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;
    public Collider2D doorCollider;

    private bool isOpen = false;

    public void Toggle()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            spriteRenderer.sprite = openSprite;
            doorCollider.enabled = false;
        }
        else
        {
            spriteRenderer.sprite = closedSprite;
            doorCollider.enabled = true;
        }
    }
}