using UnityEngine;

public class SpawnFinish : MonoBehaviour
{
    public void OnSpawnFinished()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.enabled = false;

        PlayerController movement = GetComponent<PlayerController>();
        if (movement != null)
            movement.enabled = true;
    }
}
