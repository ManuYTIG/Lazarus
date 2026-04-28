using UnityEngine;
using UnityEngine.UI;

public class RepairMachine : MonoBehaviour
{
    [Header("Repair Settings")]
    public int hpBonusPerRepair = 2;

    [Header("Machine Sprites")]
    public Sprite state0;
    public Sprite state1;
    public Sprite state2;
    public Sprite state3;

    [Header("Key Prompt")]
    public GameObject keyPrefab;
    public Texture2D buttonTexture;
    private Image keyImage;
    private Transform keyInstance;

    [Header("Sound Effects")]
    public AudioClip installSound;
    public AudioClip finalActivationSound;
    private AudioSource audioSource;

    private SpriteRenderer sr;
    private bool playerInRange = false;
    private PlayerController player;
    private Health playerHealth;

    private bool coreInstalled = false;
    private bool stickInstalled = false;
    private bool thirdInstalled = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.spatialBlend = 0f;
        sr = GetComponent<SpriteRenderer>();
        if (state0 != null) sr.sprite = state0;

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
            playerHealth = other.GetComponent<Health>();
            keyInstance.gameObject.SetActive(true);
        }
    }

   private void PlaySound(AudioClip clip)
    {
    if (clip != null && audioSource != null)
        audioSource.PlayOneShot(clip, 0.5f); // change 0.5f pour ajuster
    
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            playerHealth = null;
            keyInstance.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || player == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemData item = player.inventorySystem.Getitem();

            if (item == null)
            {
                if (stickInstalled && !thirdInstalled)
                    ActivateFinal();
                return;
            }

            if (item.ID == "core_item" && !coreInstalled)
                InstallCore(item);
            else if (item.ID == "light_item" && !stickInstalled && coreInstalled)
                InstallStick(item);
        }
    }

    private void InstallCore(ItemData item)
    {
        PlaySound(installSound);
        player.inventorySystem.ConsumeItem(item);
        coreInstalled = true;
        ApplyHPBonus();
        if (state1 != null) sr.sprite = state1;
        CameraFollow.instance?.TriggerShake(0.4f, 0.2f);
        MemoryManager.instance?.OnRepair(1);
        Debug.Log("[Machine] Core installé — Réparation 1");
    }

    private void InstallStick(ItemData item)
    {
        PlaySound(installSound);
        player.inventorySystem.ConsumeItem(item);
        stickInstalled = true;
        ApplyHPBonus();
        if (state2 != null) sr.sprite = state2;
        CameraFollow.instance?.TriggerShake(0.4f, 0.2f);
        MemoryManager.instance?.OnRepair(2);
        Debug.Log("[Machine] Barre installée — Réparation 2");
    }

    private void ActivateFinal()
    {
        PlaySound(installSound);
        thirdInstalled = true;
        ApplyHPBonus();
        if (state3 != null) sr.sprite = state3;
        CameraFollow.instance?.TriggerShake(0.6f, 0.3f);
        MemoryManager.instance?.OnRepair(3);
        Debug.Log("[Machine] Électricité rétablie — Machine complète");
    }

    private void ApplyHPBonus()
    {
        if (playerHealth != null)
        {
            playerHealth.maxHealth += hpBonusPerRepair;
            playerHealth.ResetHealth();
        }
    }
}
