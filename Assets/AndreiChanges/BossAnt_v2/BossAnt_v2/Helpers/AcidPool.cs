using System.Collections;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    public float duration;
    //public float damagePerSec;

    private void Start() => StartCoroutine(Lifetime());

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    // Utilise Health.RemoveHealth comme le reste du projet
    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    Health h = col.GetComponent<Health>();
    //    if (h != null) h.RemoveHealth(damagePerSec * Time.deltaTime);
    //}
}
