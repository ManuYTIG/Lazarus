using UnityEngine;

public class TrapSceneHandler : MonoBehaviour
{
    public ManageEndFalling endFallingManager;
    public GameObject canvasFalling;
    public Sprite spriteShadow;
    public GameObject player;
    public GameObject characterObject;
    public GameObject parentTemporaryFallInTrap;
    public Vector2 playerSpawn;
    public Camera cam;
    public GameManager manager;
    public TimerSystem timerSystem;
    public GameObject shadow;
    public AudioSource source;
    public GameObject hole;
    public AudioClip soundWind;
    public AudioClip soundFall;
    public GameObject skipSceneObject;
    private Sprite prevShadow;
    private float timer = 0f;
    private GameObject cloneCharacter;
    private int step = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevShadow = shadow.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.skippedScene)
        {
            manager.skippedScene = false;
            canvasFalling.SetActive(false);
            characterObject.SetActive(true);
            timer = 0f;
            step = 0;
            if(step < 4) endFallingManager.stopEndFallingAnimSkip();
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
                resetPlayer();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().enabled = false;
            Debug.Log("Player entered trap trigger");
            hole.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            cloneCharacter = Instantiate(characterObject, characterObject.GetComponent<Transform>().position, characterObject.GetComponent<Transform>().rotation);
            parentTemporaryFallInTrap.SetActive(true);
            parentTemporaryFallInTrap.transform.position = characterObject.GetComponent<Transform>().position;
            cloneCharacter.transform.SetParent(parentTemporaryFallInTrap.transform);
            cloneCharacter.transform.localPosition = Vector3.zero;
            parentTemporaryFallInTrap.GetComponent<Animator>().Play("FallInTrap");
            cam.GetComponent<CameraFollow>().enabled = false;
            movePlayer();
            step = 1;
            // Now move the player to the spawn point
            characterObject.SetActive(false);
            skipSceneObject.SetActive(true);
        }
    }

    public void startFallSequence() 
    {
        Debug.Log("step 2");
        step = 2;
        cam.transform.position = new Vector3(playerSpawn.x, playerSpawn.y, cam.transform.position.z);
        canvasFalling.SetActive(true);
        source.clip = soundWind;
        source.Play();
        shadow.GetComponent<SpriteRenderer>().sprite = spriteShadow;
        timer = 5f;
        
    }

    void movePlayer()
    {
        player.transform.position = playerSpawn;
    }


    public void EndFallPlayer()
    {
        shadow.GetComponent<SpriteRenderer>().sprite = prevShadow;
        step = 4;
        timer = 3f;
        source.Stop();
        source.clip = soundFall;
        source.PlayOneShot(soundFall);
        Debug.Log("step 4");
        characterObject.SetActive(true);
        characterObject.GetComponent<Animator>().Play("fallen_player");
    }

    public void resetPlayer()
    {
        player.GetComponent<Health>().RemoveHealth(player.GetComponent<Health>().health - 10);
        characterObject.GetComponent<Animator>().Play("character_front");
        player.GetComponent<PlayerController>().enabled = true;
        shadow.GetComponent<SpriteRenderer>().sprite = prevShadow;
        cam.GetComponent<CameraFollow>().enabled = true;
        timerSystem.StartTimer(); 
        skipSceneObject.SetActive(false);
    }
}
