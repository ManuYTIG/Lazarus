using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ChargingMachine : MonoBehaviour
{
    [Header("Charged Item")]
    public ItemData chargedLightItem;

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
    public Image flashImage;

    [Header("Sprites")]
    public SpriteRenderer renderer;
    public Sprite openedSprite;
    public Sprite chargingSprite;
    public Sprite chargedSprite;

    [Header("Lights")]
    public Light2D coreLight;
    public Light2D cristalLight;
    public Light2D smallLight;
    public Light2D screenLight;
    public Color cristalEnd;
    public Color cristalStart;
    public Color screenColor;
    public Color smallGreen;
    public Color smallCyan;
    public float startIntensity = 0.5f;
    public float endIntensity = 1.5f;
    public float defaultIntensity = 1f;



    private bool playerInRange = false;
    private PlayerController player;
    private InventorySystem inventory;
    private bool isCharging = false;

    void Start()
    {
        coreLight.enabled = false;
        cristalLight.enabled = false;
        smallLight.enabled = true;
        screenLight.enabled = true;
        renderer.sprite = openedSprite;
        smallLight.color = smallCyan;

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
            if (item != null && (item.ID == "light_item" || item.ID == "light_item_charged"))
                StartCoroutine(ChargeRoutine(item));
        }
    }

    private IEnumerator ChargeRoutine(ItemData item)
    {
        isCharging = true;
        player.enabled = false;
        inventory.ConsumeItem(item);
        renderer.sprite = chargingSprite;
        if (chargeSound != null)
            audioSource.PlayOneShot(chargeSound, 0.5f);

        CameraFollow.instance?.TriggerShake(chargeDuration, 0.15f);
        StartCoroutine(FlashRoutine(chargeDuration));

        StartCoroutine(LightChargeRoutine(chargeDuration));

        yield return new WaitForSeconds(chargeDuration);

        if (chargedLightItem != null)
            inventory.AddItem(chargedLightItem);

        player.enabled = true;
        isCharging = false;
        renderer.sprite = openedSprite;
    }

    private IEnumerator FlashRoutine(float duration)
    {
        Debug.Log($"FlashRoutine started, flashImage: {(flashImage != null ? flashImage.name : "NULL")}");
        if (flashImage == null) yield break;

        float half = duration / 2f;
        float t = 0f;

        while (t < half)
        {
            t += Time.deltaTime;
            flashImage.color = new Color(1f, 0.9f, 0f, Mathf.Lerp(0f, 0.6f, t / half));
            yield return null;
        }

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            flashImage.color = new Color(1f, 0.9f, 0f, Mathf.Lerp(0.6f, 0f, t / half));
            yield return null;
        }

        flashImage.color = new Color(1f, 0.9f, 0f, 0f);
    }
    private IEnumerator LightChargeRoutine(float duration)
    {
        float t = 0f;

        // Enable lights at start
        coreLight.enabled = true;
        cristalLight.enabled = true;
        cristalLight.color = cristalStart;
        screenLight.enabled = true;
        smallLight.enabled = true;
        smallLight.color = smallCyan;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            // Intensity
            float intensity = Mathf.Lerp(startIntensity, endIntensity, lerp);
            coreLight.intensity = intensity;
            cristalLight.intensity = intensity;
            screenLight.intensity = intensity;

            yield return null;
        }

        // Clamp final values
        coreLight.enabled = false;
        cristalLight.enabled = false;
        screenLight.intensity = defaultIntensity;
        smallLight.color = smallGreen;
        cristalLight.color = cristalEnd;
    }

}