using UnityEngine;

public class DialogueSceneLab : MonoBehaviour
{
    public GameObject sceneObject;
    public GameManager manager;
    public Sprite robotSprite;
    public Sprite playerSprite;
    public Vector2 desiredPosition;
    public float desiredScale;
    public GameObject player;
    public AudioClip audioRobot;
    public AudioClip audioPlayer;
    public Camera camera;
    public DialogueHandler dialogueHandler;
    private int step = 0;
    private Vector3 desired3;
    private static float lerpTime = 2f;
    private float timer = 0f;
    private bool playedScene = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desired3 = new Vector3(desiredPosition.x, desiredPosition.y, camera.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.skippedScene)
        {
            player.GetComponent<PlayerController>().enabled = true;
            camera.GetComponent<ZoomHandler>().enabled = true;
            camera.GetComponent<CameraFollow>().enabled = true;
            dialogueHandler.StopDialogue();
        }

        timer -= Time.deltaTime;
        if(timer > 0f && step == 1)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, desired3, 1f - (timer / lerpTime));
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, desiredScale, 1f - (timer / lerpTime));
        }
        else if (timer <= 0f)
        {
            if (step == 1)
            {
                dialogueHandler.StartDialogue(robotSprite, "Doit résister...", 0.03f, 2f, true, audioRobot, 0.05f);
                step = 2;
            }
        }

        if (step == 2 && dialogueHandler.IsDone())
        {
            dialogueHandler.StartDialogue(robotSprite, "les résidents...", 0.03f, 2f, true, audioRobot, 0.05f);
            step = 3;
        }
        else if (step == 3 && dialogueHandler.IsDone())
        {
            dialogueHandler.StartDialogue(robotSprite, "et ce qui les avantages.", 0.03f, 2f, true, audioRobot, 0.05f);
            step = 4;
        }
        else if(step == 4 && dialogueHandler.IsDone())
        {
            dialogueHandler.StartDialogue(playerSprite, "Ce qui m'avantage?", 0.03f, 2f, true, audioPlayer, 0.05f);
            step = 5;
        } else if(step == 5 && dialogueHandler.IsDone())
        {
            player.GetComponent<PlayerController>().enabled = true;
            camera.GetComponent<ZoomHandler>().enabled = true;
            camera.GetComponent<CameraFollow>().enabled = true;
            dialogueHandler.StopDialogue();
            sceneObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Found something");
        if (!playedScene)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Starting Dialogue Lab Scene");
                sceneObject.SetActive(true);
                player.GetComponent<PlayerController>().enabled = false;
                player.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
                camera.GetComponent<ZoomHandler>().enabled = false;
                camera.GetComponent<CameraFollow>().enabled = false;
                step = 1;
                timer = lerpTime;
                playedScene = true;
            }
        }
    }
}
