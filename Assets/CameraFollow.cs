using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;

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
            smoothedPosition += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeDuration -= Time.deltaTime;
        }

        transform.position = smoothedPosition;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
