using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject MenuUI;
    public GameObject SettingsUI;
    public GameObject GameUI;
    public GameObject inGameUI;
    public GameObject dyingCharacter;
    public GameObject Timer;
    public Camera cam;
    public GameObject skipSceneObject;
    private ZoomHandler zoomHandle;
    private TimerSystem timerSystem;
    private PlayerRespawn playerRespawn;
    private bool startedGame;
    private PlayerController movement;
    private Animator anim;
    private DyingCharacterSceneHandler dyingCharacterSceneHandler;
    private int numRespawns = 0;
    private int currentSceneIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoomHandle = cam.GetComponent<ZoomHandler>();
        playerRespawn = player.GetComponent<PlayerRespawn>();
        timerSystem = Timer.GetComponent<TimerSystem>();
        dyingCharacterSceneHandler = dyingCharacter.GetComponent<DyingCharacterSceneHandler>(); 
        anim = player.GetComponent<Animator>();
        movement = player.GetComponent<PlayerController>();
        skipSceneObject.SetActive(false);
        setMenuUIScreen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SkipScene()
    {
        if (currentSceneIndex == 0)
        {
            Debug.Log("Skipping first scene");
            dyingCharacterSceneHandler.StopScene();
        }

        skipSceneObject.SetActive(false);
        StartGamePlay();
    }

    public void startGame()
    {
        MenuUI.SetActive(false);
        SettingsUI.SetActive(false);
        inGameUI.SetActive(false);
        if (!startedGame)
        {
            Debug.Log("First game start");
            playerRespawn.Respawn();
        } else
        {
            movement.enabled = true;
        }
    }

    public void setSettingsUIScreen()
    {
        GameUI.SetActive(false);
        if(movement != null)
            movement.enabled = false;
        else Debug.LogWarning("PlayerController component not found on player object.");
        MenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void setMenuUIScreen()
    {
        SettingsUI.SetActive(false);
        GameUI.SetActive(false);
        movement.enabled = false;
        MenuUI.SetActive(true);
    }

    public void OnSpawnFinished()
    {
        if (numRespawns == 0)
        {
            skipSceneObject.SetActive(true);
            currentSceneIndex = 0;
            Debug.Log("First respawn");
            zoomHandle.enabled = false;
            dyingCharacterSceneHandler.StartDyingSequence();
        }
        else if (movement != null)
        {
            Debug.Log("Random respawn");
            StartGamePlay();
        }


        numRespawns++;

    }

    public void StartGamePlay()
    {
        currentSceneIndex = -1;
        Debug.Log("Starting gameplay");
        inGameUI.SetActive(true);
        zoomHandle.enabled = true;
        if(anim != null)
            anim.enabled = false;
        movement.enabled = true;
        if (timerSystem != null)
        {
            timerSystem.enabled = true;
            timerSystem.ResetTimer();
        }
    }
}
