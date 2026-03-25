using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Player
    public float smoothSpeed = 5f;    // How smooth the movement is
    public Vector3 offset = new Vector3(0, 0, -10); // Keep camera in front

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}
