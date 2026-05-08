using UnityEngine;

public class TrapSceneHandler : MonoBehaviour
{
    public ManageEndFalling endFallingManager;
    public GameObject canvasFalling;
    public GameObject player;
    public GameObject characterObject;
    public GameObject parentTemporaryFallInTrap;
    public Vector2 playerSpawn;
    public Camera cam;
    public GameManager manager;
    public TimerSystem timerSystem;
    public GameObject shadow;
    public GameObject newShadow;
    public AudioSource source;
    public GameObject hole;
    public AudioClip soundWind;
    public AudioClip soundFall;
    public GameObject skipSceneObject;
    private GameObject tempShadow;
    private float timer = 0f;
    private GameObject cloneCharacter;
    private int step = 0;
    public Hole holeScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.skippedScene && step != 0)
        {
            manager.skippedScene = false;
            canvasFalling.SetActive(false);
            characterObject.SetActive(true);
            timer = 0f;
            source.Stop();
            if (step < 4) endFallingManager.stopEndFallingAnimSkip();
            step = 0;
            resetPlayer();
        }

        timer = Mathf.Max(0, timer - Time.deltaTime);

        if(timer <= 0)
        {
            if(step == 2)
            {
                Debug.Log("step 3");
                step = 3;
                canvasFalling.SetActive(false);
                endFallingManager.playEndFallingAnim();
            }

            if(step == 4)
            {
                Debug.Log("finished");
                step = 0;
                skipSceneObject.SetActive(false);
                resetPlayer();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // if the hole is fixed, DO NOT play the cutscene
        if (holeScript != null && holeScript.isHoleFixed)
        {
            Debug.Log("Hole is fixed — skipping trap cutscene.");
            return;
        }

        //  Otherwise, play the cutscene normally
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        Debug.Log("Player entered trap trigger");
        hole.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        cloneCharacter = Instantiate(characterObject, characterObject.transform.position, characterObject.transform.rotation);
        parentTemporaryFallInTrap.SetActive(true);
        parentTemporaryFallInTrap.transform.position = characterObject.transform.position;
        parentTemporaryFallInTrap.transform.localPosition = (parentTemporaryFallInTrap.transform.localPosition + hole.transform.localPosition)/2;
        cloneCharacter.transform.SetParent(parentTemporaryFallInTrap.transform);
        cloneCharacter.transform.localPosition = Vector3.zero;
        parentTemporaryFallInTrap.GetComponent<Animator>().Play("FallInTrap");
        cam.GetComponent<CameraFollow>().enabled = false;
        movePlayer();
        step = 1;
        characterObject.SetActive(false);
        skipSceneObject.SetActive(true);
    }


    public void startFallSequence() 
    {
        if(!manager.skippedScene)
        {
            Debug.Log("step 2");
            step = 2;
            cam.transform.position = new Vector3(playerSpawn.x, playerSpawn.y, cam.transform.position.z);
            canvasFalling.SetActive(true);
            source.clip = soundWind;
            source.Play();
            tempShadow = Instantiate(newShadow, shadow.transform.position, shadow.transform.rotation);
            tempShadow.SetActive(true);
            tempShadow.transform.SetParent(player.transform);
            timer = 5f;
        }
    }

    void movePlayer()
    {
        player.transform.position = playerSpawn;
    }


    public void EndFallPlayer()
    {
        if (!manager.skippedScene)
        {
            step = 4;
            timer = 3f;
            source.Stop();
            source.clip = soundFall;
            source.PlayOneShot(soundFall);
            Debug.Log("step 4");
            characterObject.SetActive(true);
            characterObject.GetComponent<Animator>().Play("fallen_player");
        }
    }

    public void resetPlayer()
    {
        player.GetComponent<Health>().RemoveHealth(player.GetComponent<Health>().health - 10);
        Destroy(tempShadow);
        characterObject.GetComponent<Animator>().Play("character_front");
        player.GetComponent<PlayerController>().enabled = true;
        shadow.SetActive(true);
        cam.GetComponent<CameraFollow>().enabled = true;
        timerSystem.StartTimer(); 
    }
}
