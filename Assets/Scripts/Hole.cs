using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Hole : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerController player;
    private InventorySystem inventory;
    private bool hasPlank = true;

    [Header("Key Prompt")]
    public GameObject keyPrefab;
    public Texture2D buttonTexture;
    private Image keyImage;
    private Transform keyInstance;

    [Header("Sprite")]
    public SpriteRenderer renderer;
    public Sprite plankSprite;
    public Collider2D holeCollider;

    public bool isHoleFixed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sprite s = Sprite.Create(
            buttonTexture,
            new Rect(0, 0, buttonTexture.width, buttonTexture.height),
            new Vector2(0.5f, 0.5f)
        );
        keyInstance = Instantiate(keyPrefab).transform;
        keyInstance.SetParent(transform, worldPositionStays: true);
        keyInstance.localPosition = new Vector3(0, 1.2f, 0);
        keyInstance.gameObject.SetActive(false);
        keyImage = keyInstance.Find("View").GetComponent<Image>();
        keyImage.sprite = s;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerController>();
            inventory = other.GetComponent<InventorySystem>();
            if(inventory.Getitem() != null && inventory.Getitem().ID == "plank")
            {
                keyInstance.gameObject.SetActive(true);
                hasPlank = true;
            }
                
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            keyInstance.gameObject.SetActive(false);
            hasPlank = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInRange || player == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {        
            if (hasPlank)
                {
                holeCollider.enabled = false;
                renderer.sprite = plankSprite;
                inventory.ConsumeItem(inventory.Getitem());
                hasPlank = false;
                isHoleFixed = true;
                keyInstance.gameObject.SetActive(false);
            }
        }
    }


}
