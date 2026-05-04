using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimerSystem : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 120f;
    private float currentTime;
    public PlayerRespawn playerRespawn;
    public Camera MainCamera;
    private float endZoom = 0.9f;
    private float initSize;
    public Image runoutScreen;
    private bool isRunning = false;

    void Start()
    {
        initSize = MainCamera.orthographicSize;
        currentTime = startTime;
        isRunning = false;
    }

    public void StartTimer()
    {
        Debug.Log("Starting timer");
        isRunning = true;
    }

    public void Pause()
    {
        isRunning = false;
    }

    void Update()
    {
        if(isRunning)
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
            else if (currentTime < 20)
            {
                float targetSize = initSize * endZoom;
                MainCamera.orthographicSize = Mathf.Lerp(
                initSize,
                targetSize,
                Mathf.Min(20f - currentTime, 10) / 10f);

                float opacity = Mathf.Min((20 - currentTime) / 15, 1f);
                Debug.Log($"Setting opacity");
                runoutScreen.color = new Color(runoutScreen.color.r, runoutScreen.color.g, runoutScreen.color.b, opacity);
            }
        }
    }

    public void ResetTimer()
    {
        ResetTimer(startTime);
    }

    public void ResetTimer(float time)          // <-- added so Respawn can reset the time
    {
        currentTime = time;
    }
}