using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChargingMachine : MonoBehaviour
{
    [Header("Charged Item")]
    public ItemData chargedLightItem; // ItemData de la barre illuminée

    [Header("Charge Settings")]
    public float chargeDuration = 5f;

    [Header("Sound")]
    public AudioClip chargeSound;
    private AudioSource audioSource;

    [Header("Key Prompt")]
    public GameObject keyPrefab;
    public Texture2D buttonTexture;
    private Image keyImage;
    private Transform keyInstance;

    [Header("Screen Flash")]
    public GameObject flashOverlayPrefab; // un Panel UI jaune qu'on va créer

    private bool playerInRange = false;
    private PlayerController player;
    private InventorySystem inventory;
    private bool isCharging = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;

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
            keyInstance.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            inventory = null;
            keyInstance.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || player == null || isCharging) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemData item = inventory.Getitem();
            if (item != null && item.ID == "light_item")
                StartCoroutine(ChargeRoutine(item));
        }
    }

    private IEnumerator ChargeRoutine(ItemData item)
    {
        isCharging = true;

        // Désactive le joueur
        player.enabled = false;

        // Consomme la barre
        inventory.ConsumeItem(item);

        // Son
        if (chargeSound != null)
            audioSource.PlayOneShot(chargeSound, 0.5f);

        // Flash jaune
        GameObject flash = null;
        if (flashOverlayPrefab != null)
        {
            flash = Instantiate(flashOverlayPrefab);
            StartCoroutine(FlashRoutine(flash, chargeDuration));
        }

        // Camera shake pendant le chargement
        CameraFollow.instance?.TriggerShake(chargeDuration, 0.15f);

        yield return new WaitForSeconds(chargeDuration);

        // Redonne la barre chargée
        if (chargedLightItem != null)
            inventory.AddItem(chargedLightItem);

        // Réactive le joueur
        player.enabled = true;

        if (flash != null)
            Destroy(flash);

        isCharging = false;
    }

    private IEnumerator FlashRoutine(GameObject flash, float duration)
    {
        Image img = flash.GetComponentInChildren<Image>();
        if (img == null) yield break;

        float half = duration / 2f;

        // Fade in
        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            img.color = new Color(1f, 0.9f, 0f, Mathf.Lerp(0f, 0.6f, t / half));
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            img.color = new Color(1f, 0.9f, 0f, Mathf.Lerp(0.6f, 0f, t / half));
            yield return null;
        }
    }
}
