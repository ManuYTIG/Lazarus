using UnityEngine;

public class CoreItem : MonoBehaviour
{
    [Header("Effects (à brancher plus tard)")]
    public AudioClip pickupSound;
    public float screenShakeDuration = 0.3f;
    public string memoryText = "...quelque chose de familier.";

    public void OnPickedUp()
    {
        PlaySound();
        TriggerScreenEffect();
        ShowText();
    }

    private void PlaySound()
    {
    if (pickupSound != null)
        AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);
    }

    private void TriggerScreenEffect()
{
    if (CameraFollow.instance != null)
        CameraFollow.instance.TriggerShake(screenShakeDuration, 0.15f);
}


    private void ShowText()
    {
        // TODO: afficher memoryText via ton UI manager
        Debug.Log($"[CoreItem] Memory text: {memoryText}");
    }
}
