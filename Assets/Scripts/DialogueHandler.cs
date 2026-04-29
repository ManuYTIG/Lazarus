using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public GameObject TextDialogue;
    public GameObject CharacterImage;
    private Image dialogueCharacterImg;
    private TextMeshProUGUI dialogueText;
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
            if(timer >= charSpeed)
            {
                if(dialogueText.text.Length < fullText.Length)
                {
                    dialogueText.text += fullText[dialogueText.text.Length];
                    timer = 0f;
                }
                else
                {
                    if(timer > timeAfterDialogue)
                    {
                        timer = 0f;
                        isTyping = false;
                        if(!stayActive) gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
