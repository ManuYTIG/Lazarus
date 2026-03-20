using UnityEngine;

public class SpawnAnimationHandler : MonoBehaviour
{
    public GameObject playerObject; // assign your real player in Inspector
    public GameObject MainCameraSpawn;

    // This function will be called by the Animation Event
    public void OnSpawnAnimationFinished()
    {
        if (playerObject != null)
        {
            playerObject.SetActive(true);
        }

        // Destroy the spawn visual after animation ends
        Destroy(gameObject);
        Destroy(MainCameraSpawn);
    }
}
