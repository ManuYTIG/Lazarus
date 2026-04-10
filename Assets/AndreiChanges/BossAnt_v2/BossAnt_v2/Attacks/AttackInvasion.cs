using System.Collections;
using UnityEngine;

/// <summary>
/// Phase 3 — Invasion totale : spawn continu d'enfants jusqu'à la mort du boss.
/// Assigne le tag "Minion" à tes prefabs d'enfants dans Unity.
/// </summary>
public class AttackInvasion : BaseAttack
{
    [Header("Invasion")]
    public GameObject[] minionPrefabs;
    public float        spawnInterval = 1.2f;
    public float        totalDuration = 20f;
    public float        spawnRadius   = 4f;
    public int          maxMinions    = 12;

    public Animator  bossAnimator;
    public AudioClip invasionSound;
    private AudioSource audioSource;
    private void Awake() => audioSource = GetComponent<AudioSource>();

    protected override IEnumerator DoAttack(BossController boss)
    {
        Debug.Log("BossAnt: AttackInvasion");
        bossAnimator?.SetTrigger("Roar");
        audioSource?.PlayOneShot(invasionSound);
        yield return new WaitForSeconds(0.5f);

        float elapsed = 0f;
        while (elapsed < totalDuration && !boss.IsDead)
        {
            int current = GameObject.FindGameObjectsWithTag("Minion").Length;
            if (current < maxMinions && minionPrefabs != null && minionPrefabs.Length > 0)
            {
                Vector2    offset   = Random.insideUnitCircle.normalized * spawnRadius;
                Vector3    pos      = boss.transform.position + new Vector3(offset.x, offset.y, 0);
                GameObject prefab   = minionPrefabs[Random.Range(0, minionPrefabs.Length)];
                Instantiate(prefab, pos, Quaternion.identity);
            }
            elapsed += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
