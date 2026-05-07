using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public TrapSceneHandler trapSceneHandler;
    public void EndFallInTrap()
    {
        gameObject.SetActive(false);
        trapSceneHandler.startFallSequence();
    }
}
