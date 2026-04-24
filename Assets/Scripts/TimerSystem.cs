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
    private float endZoom = 0.5f;
    private float zoomSpeed = 0.1f;
    private float initSize;
    public RawImage runoutScreen;
    private float endOpacity = 0.2f;
    private bool isLerping = false;

    void Start()
    {
        initSize = MainCamera.orthographicSize;
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
            isLerping = false;
        } else if(currentTime < 10) {
            if(!isLerping) {
                Debug.Log($"Lerping from {initSize} to {targetSize}");
                isLerping = true;
                float targetSize = initSize * endZoom;
            
            MainCamera.orthographicSize = Mathf.Lerp(
            initSize,
            10,
            targetSize
        );
            }
            
            float opacity = (endOpacity * (10 - currentTime)/10);
            runoutScreen.color = new Color(runoutScreen.color.r, runoutScreen.color.g, runoutScreen.color.b, opacity);
        }
    }

    public void ResetTimer()          // <-- added so Respawn can reset the time
    {
        MainCamera.orthographicSize = initSize;
        currentTime = startTime;
        isLerping = false;
    }
}