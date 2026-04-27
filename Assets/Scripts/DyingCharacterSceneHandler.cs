using UnityEngine;

public class DyingCharacterSceneHandler : MonoBehaviour
{
    public GameObject player;
    public Camera cam;
    public GameObject dialogueUI;
    public Sprite spritePlayer;
    public Sprite spriteDyingCharacter;
    public GameObject Timer;
    public GameObject manager;
    private GameManager gameManager;
    private TimerSystem timerSystem;
    private DialogueHandler dialogueHandler;
    private GameObject bloodPool;
    private PlayerController playerController;
    private BloodPoolManager bloodPoolManager;
    private int step = 0;
    private float timer = 0f;
    private float zoomSpeed = 0.5f;
    private float prevSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueHandler = dialogueUI.GetComponent<DialogueHandler>();
        gameManager = manager.GetComponent<GameManager>();
        timerSystem = Timer.GetComponent<TimerSystem>();
        bloodPool = GameObject.Find("ExpandingBloodPool");
        playerController = player.GetComponent<PlayerController>();
        prevSize = cam.orthographicSize;

        if (bloodPool != null)
        {
            bloodPoolManager = bloodPool.GetComponent<BloodPoolManager>();
        }
    }

    public void StartDyingSequence()
    {
        playerController.enabled = false;
        step = 1;
        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (step > 0)
        {
            prevSize = cam.orthographicSize;
            timer -= Time.deltaTime;
            float targetSize = 6f; // zoom to target size
            if (step == 1)
            {
                Animator animBlood = bloodPool.GetComponent<Animator>();
                animBlood.Play("ExpandingBloodPool");
                cam.orthographicSize = Mathf.Lerp(
                    prevSize,
                    targetSize,
                    (1 - timer) * 0.5f
                );
                if (timer <= 0f)
                {
                    step = 2;
                    timer = 0f;
                }
            }
            else if (step == 2)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 3;
                    if (bloodPoolManager != null)
                    {
                        bloodPoolManager.StartBloodPool();
                    }
                    dialogueHandler.StartDialogue(spritePlayer, "Où suis-je ?", 0.05f, true);
                }
            }
            else if (step == 3)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 4;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "Tu ne te rappelle pas ?", 0.03f, true);
                }
            }
            else if (step == 4)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 5;
                    dialogueHandler.StartDialogue(spritePlayer, "...", 0.2f, true);
                }
            }
            else if (step == 5)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 6;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "Pas besoin de répondre. Je comprends.", 0.03f, true);
                }
            }
            else if (step == 6)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 7;
                    dialogueHandler.StartDialogue(spritePlayer, "Comprends quoi ?", 0.03f, true);
                }
            }
            else if (step == 7)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 8;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "Tu vas le comprendre plus tard...", 0.03f, 3f, true);
                }
            }
            else if (step == 8)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 9;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "Promets-moi...", 0.1f, true);
                }
            }
            else if (step == 9)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 10;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "Que quand tu comprendras...", 0.1f, true);
                }
            }
            else if (step == 10)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 11;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "Tu vas me rejoindre...", 0.1f, 3f, true);
                    Animator anim = gameObject.GetComponent<Animator>();
                    anim.Play("DeadCharacter");
                    Animator animBlood = bloodPool.GetComponent<Animator>();
                    animBlood.Play("BloodPoolEnd");
                }
            }
            else if (step == 11)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 12;
                    dialogueHandler.StartDialogue(spritePlayer, "Tu veux dire mourrir ?", 0.1f, true);
                }
            }
            else if (step == 12)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 13;
                    dialogueHandler.StartDialogue(spriteDyingCharacter, "...", 0.5f, true);
                }
            }
            else if (step == 13)
            {
                if (dialogueHandler.IsDone())
                {
                    step = 14;
                    dialogueHandler.StartDialogue(spritePlayer, "Il est mort...", 0.1f, 1f);
                    timer = 1f;
                }
            }
            else if (step == 14)
            {
                if(dialogueHandler.IsDone()) 
                { 
                    step = 15;
                }
            } else if (step == 15)
            {
                cam.orthographicSize = Mathf.Lerp(
                    targetSize,
                    prevSize,
                    (1 - timer) * 0.5f
                );

                if (timer <= 0f)
                {
                    Debug.Log("Dying sequence ended, starting gameplay...");
                    step = 0;
                    gameManager.StartGamePlay();
                    //end of sequence, you can add any additional logic here
                }
            }
        }
    }
}
