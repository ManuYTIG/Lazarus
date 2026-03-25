using UnityEngine;

public class KeypadVisibility : MonoBehaviour
{
    public GameObject UIKeypad;

    private bool isVisible = false;

    void Start()
    {
        UIKeypad.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isVisible = !isVisible;
            UIKeypad.SetActive(isVisible);
        }
    }
}