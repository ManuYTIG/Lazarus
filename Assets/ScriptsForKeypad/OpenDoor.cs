using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(false); // hides the door
    }
}