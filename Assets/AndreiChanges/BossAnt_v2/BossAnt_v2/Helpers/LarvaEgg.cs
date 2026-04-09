using System.Collections;
using UnityEngine;

public class LarvaEgg : MonoBehaviour
{
    public float      hatchDelay   = 3f;
    public GameObject minionPrefab;
    public GameObject hatchVFX;

    private void Start() => StartCoroutine(Hatch());

    private IEnumerator Hatch()
    {
        yield return new WaitForSeconds(hatchDelay);
        if (hatchVFX     != null) Instantiate(hatchVFX,     transform.position, Quaternion.identity);
        if (minionPrefab != null) Instantiate(minionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
