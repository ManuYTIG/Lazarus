using UnityEngine;

public class SimpleSoundRepeater : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] float TimeDelay;
    void Start()
    {
        // Parameters: (Method Name, Initial Delay, Repeat Rate)
        InvokeRepeating("PlaySound", 0f, TimeDelay);
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}