using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;
    private static float shakeSpeed = 5f;

    public static CameraFollow instance;

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        if (shakeDuration > 0)
        {
            smoothedPosition += (Vector3)(new Vector3(Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * 2 - 1, Mathf.PerlinNoise(0, Time.time * shakeSpeed) * 2 - 1, 0) * shakeMagnitude * 0.1f);
            shakeDuration -= Time.deltaTime;
        }

        transform.position = smoothedPosition;
    }

    public void TriggerShake(float duration, float magnitude = 0.5f)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    public void StartIndefiniteShake(float magnitude = 0.5f)
    {
        shakeDuration = 1000000f;
        shakeMagnitude = magnitude;
    }

    public void StopShake()
    {
        shakeDuration = 0f;
        shakeMagnitude = 0f;
    }
}
