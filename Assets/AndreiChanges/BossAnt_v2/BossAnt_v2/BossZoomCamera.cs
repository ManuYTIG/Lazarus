using UnityEngine;

public class BossZoomCamera : MonoBehaviour
{
    [Header("References")]
    public BossController boss;       // assign in Inspector
    public Camera cam;                // assign main camera

    [Header("Zoom Settings")]
    public float normalSize = 5f;
    public float bossSize = 8f;
    public float zoomSpeed = 2f;

    private void Reset()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (boss == null || cam == null) return;

        float targetSize = boss.PlayerInRange() ? bossSize : normalSize;

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetSize,
            Time.deltaTime * zoomSpeed
        );
    }
}