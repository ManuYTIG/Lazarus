using UnityEngine;
using TMPro;

public class TimerSystem : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign your UI text here in the Inspector
    public float startTime = 160f;
    private float currentTime;

    void Start() {
        currentTime = startTime;
    }

    void Update()
    {
        // Increment time every frame
        currentTime -= Time.deltaTime;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Update the UI text (Format: 00:00)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
