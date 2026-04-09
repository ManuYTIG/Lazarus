using System.Collections;
using UnityEngine;

/// <summary>
/// Appel de la colonie : fait spawner les enfants autour du boss.
/// </summary>
public class AttackSpawnMinions : BaseAttack
{
    [Header("Spawn")]
    public GameObject[] minionPrefabs;
    public int          spawnCount  = 4;
    public float        spawnRadius = 3f;
    public float        spawnDelay  = 0.2f;

    public Animator  bossAnimator;
    public AudioClip roarSound;
    private AudioSource audioSource;
    private void Awake() => audioSource = GetComponent<AudioSource>();

    protected override IEnumerator DoAttack(BossController boss)
    {
        bossAnimator?.SetTrigger("Roar");
        audioSource?.PlayOneShot(roarSound);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            if (minionPrefabs == null || minionPrefabs.Length == 0) break;
            Vector2    offset    = Random.insideUnitCircle * spawnRadius;
            Vector3    spawnPos  = boss.transform.position + new Vector3(offset.x, offset.y, 0);
            GameObject prefab    = minionPrefabs[Random.Range(0, minionPrefabs.Length)];
            Instantiate(prefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(0.3f);
    }
}
