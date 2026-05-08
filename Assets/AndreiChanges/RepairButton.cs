using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RepairButton : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTarget;
    public float cameraMoveSpeed = 3f;

    [Header("UI")]
    public GameObject promptUI;
    public Image flashImage;

    [Header("Text")]
    public string promptText = "Réparer la machine — vivre éternellement";

    [Header("Machine")]
    public RepairMachine repairMachine;

    [Header("Key Prompt")]
    public GameObject keyPrefab;
    public Texture2D buttonTexture;
    private Image keyImage;
    private Transform keyInstance;

    private bool playerInRange = false;
    private bool hasActivated = false;
    private PlayerController player;
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = CameraFollow.instance;

        if (promptUI != null)
        {
            var txt = promptUI.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = promptText;
            promptUI.SetActive(false);
        }

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
            keyInstance.gameObject.SetActive(true);
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            keyInstance.gameObject.SetActive(false);
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || player == null || hasActivated) return;

        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(ActivateRoutine());
    }

    private IEnumerator ActivateRoutine()
{
    hasActivated = true;
    player.enabled = false;
    keyInstance.gameObject.SetActive(false);
    if (promptUI != null) promptUI.SetActive(false);

    // 1. Déplace la caméra vers la cible
    if (cameraTarget != null)
    {
        cameraFollow.enabled = false;
        Camera cam = Camera.main;
        Vector3 target = new Vector3(cameraTarget.position.x, cameraTarget.position.y, cam.transform.position.z);

        while (Vector3.Distance(cam.transform.position, target) > 0.05f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        cam.transform.position = target;
    }

    yield return new WaitForSeconds(1f);

    // 2. Répare la machine + Flash bleu en même temps
    MemoryManager.instance?.OnRepair(3);
    if (repairMachine != null) repairMachine.TriggerFinalRepair();
    StartCoroutine(FlashRoutine(3f));

    yield return new WaitForSeconds(2f);

    // 3. Réactive CameraFollow + shake qui grandit
    cameraFollow.enabled = true;
    StartCoroutine(BuildingShakeRoutine(4f));

yield return new WaitForSeconds(4f);

    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
}

    private IEnumerator BuildingShakeRoutine(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float magnitude = Mathf.Lerp(0.05f, 1.5f, t / duration);
            CameraFollow.instance?.TriggerShake(0.05f, magnitude);
            yield return null;
        }
    }

    private IEnumerator FlashRoutine(float duration)
{
    if (flashImage == null) yield break;

    float fadeInDuration = 0.1f; // presque instant
    float fadeOutDuration = duration - fadeInDuration;
    float t = 0f;

    // Fade in rapide
    while (t < fadeInDuration)
    {
        t += Time.deltaTime;
        flashImage.color = new Color(0f, 0.5f, 1f, Mathf.Lerp(0f, 0.8f, t / fadeInDuration));
        yield return null;
    }

    // Fade out lent
    t = 0f;
    while (t < fadeOutDuration)
    {
        t += Time.deltaTime;
        flashImage.color = new Color(0f, 0.5f, 1f, Mathf.Lerp(0.8f, 0f, t / fadeOutDuration));
        yield return null;
    }

    flashImage.color = new Color(0f, 0.5f, 1f, 0f);
}
}