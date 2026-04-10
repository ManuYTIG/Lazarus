using UnityEngine;

public class ButtonRotator : MonoBehaviour
{
    public void RotateButton()
    {
        // Rotates 90 degrees around the Z-axis (standard for 2D UI)
        transform.Rotate(0, 0, 90);
    }
}
