using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{
    public GameObject doorLeft;
    public GameObject doorRight;
    public float openDistance = 1f;
    public float openTime = 1f;
    private Vector3 initialPosLeft;
    private Vector3 initialPosRight;
    private float timer = 0f;
    private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosLeft = doorLeft.transform.localPosition;
        initialPosRight = doorRight.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if(isOpen)
            {
                doorLeft.transform.localPosition = new Vector3(Mathf.Lerp(doorLeft.transform.localPosition.x,
                    initialPosLeft.x - openDistance, 1f - (timer / openTime)), doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);
                doorRight.transform.localPosition = new Vector3(Mathf.Lerp(doorRight.transform.localPosition.x,
                    initialPosRight.x + openDistance, 1f - (timer / openTime)), doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);
            } else
            {
                doorLeft.transform.localPosition = new Vector3(Mathf.Lerp(doorLeft.transform.localPosition.x,
                    initialPosLeft.x, 1f - (timer / openTime)), doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);
                doorRight.transform.localPosition = new Vector3(Mathf.Lerp(doorRight.transform.localPosition.x,
                    initialPosRight.x, 1f - (timer / openTime)), doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);
            }
            


        }
    }

    public void OpenDoor()
    {
        timer = openTime;
        isOpen = true;
    }

    public void ReinitializeDoor()
    {
        doorLeft.transform.localPosition = initialPosLeft;
        doorRight.transform.localPosition = initialPosRight;
        isOpen = false;
        timer = openTime;
    }
}
