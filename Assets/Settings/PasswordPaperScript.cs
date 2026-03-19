using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject paperUI;      // The paper panel
    public GameObject promptUI;     // "Press E to Read"
    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 3f;

    public MonoBehaviour playerMovement; // drag your movement script
    public MonoBehaviour cameraLook;     // drag your camera script

    private GameObject player;
    private bool isOpen = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (paperUI != null)
            paperUI.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Show prompt ONLY when near and not reading
        if (!isOpen && distance <= interactDistance)
        {
            if (promptUI != null)
                promptUI.SetActive(true);
        }
        else
        {
            if (promptUI != null)
                promptUI.SetActive(false);
        }

        // Handle interaction
        if (Input.GetKeyDown(interactKey))
        {
            if (isOpen)
            {
                ClosePaper();
            }
            else if (distance <= interactDistance)
            {
                OpenPaper();
            }
        }
    }

    void OpenPaper()
    {
        isOpen = true;
        paperUI.SetActive(true);
        promptUI.SetActive(false);

        // Lock player + camera
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (cameraLook != null)
            cameraLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ClosePaper()
    {
        isOpen = false;
        paperUI.SetActive(false);

        // Unlock player + camera
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (cameraLook != null)
            cameraLook.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
