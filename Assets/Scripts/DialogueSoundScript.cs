using UnityEngine;

public class DialogueSoundScript : MonoBehaviour
{
    public AudioSource source;
    private AudioClip dialogueSound;
    private float audioSpeed = 0.5f;
    private float timer = 0f;
    private bool isPlaying = false;
    public void PlayDialogueSound(AudioClip sound, float speed)
    {
        Debug.Log($"Playing dialogue sound: {sound.name} with speed: {speed}");
        isPlaying = true;
        dialogueSound = sound;
        audioSpeed = speed;
        source.clip = dialogueSound;
        timer = 0f;
    }

    public void StopDialogueSound()
    {
        Debug.Log("Stopping dialogue sound");
        dialogueSound = null;
        isPlaying = false;
        source.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > audioSpeed && dialogueSound != null && isPlaying)
        {
            source.Play();
            timer = 0f;
        }
    }
}
