using UnityEngine;

public class ManageEndFalling : MonoBehaviour
{
    public GameObject characterEndFalling;
    public TrapSceneHandler trapSceneHandler;

    public void playEndFallingAnim()
    {
        Debug.Log("Playing end falling animation");
        characterEndFalling.SetActive(true);
        gameObject.GetComponent<Animator>().Play("FallMovementEnd");
    }

    public void stopEndFallingAnim()
    {
        characterEndFalling.SetActive(false);
        trapSceneHandler.EndFallPlayer();
    }

    public void stopEndFallingAnimSkip()
    {
        characterEndFalling.GetComponent<Animator>().enabled = false;
        characterEndFalling.SetActive(false);
    }
}
