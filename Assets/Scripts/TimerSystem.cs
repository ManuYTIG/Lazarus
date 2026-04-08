using UnityEngine;
using TMPro;
public class TimerSystem : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 120f;
    private float currentTime;
    public PlayerRespawn playerRespawn;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentTime <= 0)
        {
            enabled = false;          // <-- stop timer immediately
            playerRespawn.Die();
        }
    }

    public void ResetTimer()          // <-- added so Respawn can reset the time
    {
        currentTime = startTime;
    }
}