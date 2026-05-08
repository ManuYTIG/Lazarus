using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DestroyButton : MonoBehaviour
{
    [Header("Camera Targets")]
    public Transform machineTarget;
    public Transform playerTarget;
    public float cameraMoveSpeed = 3f;

    [Header("Machine")]
    public GameObject machineObject;
    public GameObject machineExplosionParticles;

    [Header("Player Death")]
    public float bulletDelay = 1f;

    [Header("UI")]
    public Image blackOverlay;
    public Image flashImage;
    public TextMeshProUGUI endText;
    public string endTextContent = "Vous avez mis fin au cycle... était-ce le bon choix?";

    [Header("Key Prompt")]
    public GameObject keyPrefab;
    public Texture2D buttonTexture;
    private Image keyImage;
    private Transform keyInstance;

    private bool playerInRange = false;
    private bool hasActivated = false;
    private PlayerController player;
    private PlayerRespawn playerRespawn;
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = CameraFollow.instance;

        if (endText != null)
        {
            endText.text = endTextContent;
            endText.color = new Color(1f, 1f, 1f, 0f);
        }

        if (flashImage != null)
            flashImage.color = new Color(1f, 0.5f, 0f, 0f);

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
            playerRespawn = other.GetComponent<PlayerRespawn>();
            keyInstance.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            playerRespawn = null;
            keyInstance.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || player == null || hasActivated) return;

        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        hasActivated = true;
        player.enabled = false;
        keyInstance.gameObject.SetActive(false);

        // 1. Caméra vers la machine
        yield return StartCoroutine(MoveCamera(machineTarget));

        yield return new WaitForSeconds(0.5f);

        // 2. Explosion de la machine
        if (machineObject != null)
        {
            if (machineExplosionParticles != null)
            {
                GameObject p = Instantiate(machineExplosionParticles, machineObject.transform.position, Quaternion.identity);
                Destroy(p, 3f);
            }
            machineObject.SetActive(false);
        }

        StartCoroutine(FlashOrange());

        yield return new WaitForSeconds(1.5f);

        // 3. Caméra vers le joueur
        yield return StartCoroutine(MoveCamera(playerTarget));

        yield return new WaitForSeconds(bulletDelay);

        // 4. Tue le joueur sans son et sans respawn
        if (playerRespawn != null)
        {
            AudioSource playerAudio = playerRespawn.GetComponent<AudioSource>();
            if (playerAudio != null) playerAudio.mute = true;
            playerRespawn.enabled = false;
            playerRespawn.GetComponent<Health>().RemoveHealth(9999);
        }

        yield return new WaitForSeconds(1f);

        // 5. Fade to black
        yield return StartCoroutine(FadeToBlack(2f));

        // 6. Texte apparaît
        yield return StartCoroutine(FadeText(true, 1.5f));

        yield return new WaitForSeconds(5f);

        // 7. Texte disparaît
        yield return StartCoroutine(FadeText(false, 1.5f));

        // 8. Reset
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    private IEnumerator MoveCamera(Transform target)
    {
        if (target == null) yield break;
        cameraFollow.enabled = false;
        Camera cam = Camera.main;
        Vector3 dest = new Vector3(target.position.x, target.position.y, cam.transform.position.z);

        while (Vector3.Distance(cam.transform.position, dest) > 0.05f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, dest, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        cam.transform.position = dest;
    }

    private IEnumerator FadeToBlack(float duration)
    {
        if (blackOverlay == null) yield break;
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.color = new Color(0f, 0f, 0f, 0f);
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            blackOverlay.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, t / duration));
            yield return null;
        }
        blackOverlay.color = new Color(0f, 0f, 0f, 1f);
    }

    private IEnumerator FadeText(bool fadeIn, float duration)
    {
        if (endText == null) yield break;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            endText.color = new Color(1f, 1f, 1f, Mathf.Lerp(startAlpha, endAlpha, t / duration));
            yield return null;
        }
        endText.color = new Color(1f, 1f, 1f, endAlpha);
    }

    private IEnumerator FlashOrange()
    {
        if (flashImage == null) yield break;

        float fadeInDuration = 0.1f;
        float fadeOutDuration = 1.5f;
        float t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            flashImage.color = new Color(1f, 0.5f, 0f, Mathf.Lerp(0f, 0.8f, t / fadeInDuration));
            yield return null;
        }

        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            flashImage.color = new Color(1f, 0.5f, 0f, Mathf.Lerp(0.8f, 0f, t / fadeOutDuration));
            yield return null;
        }

        flashImage.color = new Color(1f, 0.5f, 0f, 0f);
    }
}