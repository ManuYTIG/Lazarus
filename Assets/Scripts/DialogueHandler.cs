using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public GameObject TextDialogue;
    public GameObject CharacterImage;
    public GameObject sourceObject;
    private Image dialogueCharacterImg;
    private AudioClip audio;
    private float speedAudio = 0.5f;
    private TextMeshProUGUI dialogueText;
    private DialogueSoundScript dialogueSoundScript;
    private float charSpeed = 0.05f;
    private string fullText;
    private float timer;
    private bool isTyping;
    private float timeAfterDialogue = 2f;
    private bool stayActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find necessary components
        dialogueText = TextDialogue.GetComponent<TextMeshProUGUI>();
        dialogueCharacterImg = CharacterImage.GetComponent<Image>();
        dialogueSoundScript = sourceObject.GetComponent<DialogueSoundScript>();
        gameObject.SetActive(false);
    }

    public void StopDialogue()
    {
        dialogueText.text = "";
        isTyping = false;
        timer = 0f;
        stayActive = false;
        gameObject.SetActive(false);
    }

    public void StartDialogue(Sprite characterSprite, string dialogue, float charSpeed)
    {
        if(isTyping) {
            Debug.Log("Already typing, ignoring new dialogue");
            return;
        } else
        {
            Debug.Log($"Starting dialogue for text: {dialogue}");
            dialogueCharacterImg.sprite = characterSprite;
            fullText = dialogue;
            dialogueText.text = "";
            isTyping = true;
            this.charSpeed = charSpeed;
            timeAfterDialogue = 2f;
            timer = 0f;
            audio = null;
            stayActive = false;
            gameObject.SetActive(true);
        }
            
    }

    public void StartDialogue(Sprite characterSprite, string dialogue, float charSpeed, float timerAfter)
    {
        if (isTyping)
        {
            Debug.Log("Already typing, ignoring new dialogue");
            return;
        }
        else
        {
            Debug.Log($"Starting dialogue for text: {dialogue}");
            dialogueCharacterImg.sprite = characterSprite;
            fullText = dialogue;
            timeAfterDialogue = timerAfter;
            audio = null;
            dialogueText.text = "";
            isTyping = true;
            this.charSpeed = charSpeed;
            timer = 0f;
            stayActive = false;
            gameObject.SetActive(true);
        }
    }

    public void StartDialogue(Sprite characterSprite, string dialogue, float charSpeed, bool stay)
    {
        if (isTyping)
        {
            Debug.Log("Already typing, ignoring new dialogue");
            return;
        }
        else
        {
            Debug.Log($"Starting dialogue for text: {dialogue}");
            dialogueCharacterImg.sprite = characterSprite;
            audio = null;
            fullText = dialogue;
            dialogueText.text = "";
            isTyping = true;
            this.charSpeed = charSpeed;
            timeAfterDialogue = 2f;
            timer = 0f;
            stayActive = stay;
            gameObject.SetActive(true);
        }

    }

    public void StartDialogue(Sprite characterSprite, string dialogue, float charSpeed, float timerAfter, bool stay)
    {
        if (isTyping)
        {
            Debug.Log("Already typing, ignoring new dialogue");
            return;
        }
        else
        {
            Debug.Log($"Starting dialogue for text: {dialogue}");
            dialogueCharacterImg.sprite = characterSprite;
            audio = null;
            fullText = dialogue;
            timeAfterDialogue = timerAfter;
            dialogueText.text = "";
            isTyping = true;
            this.charSpeed = charSpeed;
            timer = 0f;
            stayActive = stay;
            gameObject.SetActive(true);
        }
    }

    public void StartDialogue(Sprite characterSprite, string dialogue, float charSpeed, bool stay, AudioClip audioDialogue, float audioSpeed)
    {
        if (isTyping)
        {
            Debug.Log("Already typing, ignoring new dialogue");
            return;
        }
        else
        {
            Debug.Log($"Starting dialogue for text: {dialogue}");
            dialogueCharacterImg.sprite = characterSprite;
            audio = audioDialogue;
            speedAudio = audioSpeed;
            dialogueSoundScript.PlayDialogueSound(audioDialogue, audioSpeed);
            fullText = dialogue;
            dialogueText.text = "";
            isTyping = true;
            this.charSpeed = charSpeed;
            timeAfterDialogue = 2f;
            timer = 0f;
            stayActive = stay;
            gameObject.SetActive(true);
        }

    }

    public void StartDialogue(Sprite characterSprite, string dialogue, float charSpeed, float timerAfter, bool stay, AudioClip audioDialogue, float audioSpeed)
    {
        if (isTyping)
        {
            Debug.Log("Already typing, ignoring new dialogue");
            return;
        }
        else
        {
            Debug.Log($"Starting dialogue for text: {dialogue}");
            dialogueCharacterImg.sprite = characterSprite;
            audio = audioDialogue;
            speedAudio = audioSpeed;
            dialogueSoundScript.PlayDialogueSound(audioDialogue, audioSpeed);
            fullText = dialogue;
            timeAfterDialogue = timerAfter;
            dialogueText.text = "";
            isTyping = true;
            this.charSpeed = charSpeed;
            timer = 0f;
            stayActive = stay;
            gameObject.SetActive(true);
        }
    }

    public bool IsDone()
    {
        return !isTyping;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTyping)
        {
            timer += Time.deltaTime;
            Debug.Log("r");
            if (timer >= charSpeed)
            {
                if (dialogueText.text.Length < fullText.Length)
                {
                    Debug.Log($"at {dialogueText.text.Length}");
                    dialogueText.text += fullText[dialogueText.text.Length];
                    timer = 0f;
                }
                else
                {
                    if(timer > timeAfterDialogue)
                    {
                        Debug.Log("Stopping Dialogue");
                        dialogueText.text = "";
                        dialogueSoundScript.StopDialogueSound();
                        timer = 0f;
                        isTyping = false;
                        if(!stayActive) gameObject.SetActive(false);
                    }
                }
            }
            Debug.Log("s");
        }
    }
}
