using System.Collections;
using UnityEngine;

/// <summary>
/// Phase 3 — Croix tournoyante de 4 projectiles de feu qui s'écartent.
/// </summary>
public class AttackFireCross : BaseAttack
{
    [Header("Croix de feu")]
    public GameObject fireBulletPrefab;
    public float      rotationSpeed  = 120f;  // degrés/sec
    public float      crossDuration  = 3.5f;
    public float      startRadius    = 1.5f;
    public float      expansionSpeed = 1.8f;  // unités/sec
    public float      damage         = 18f;

    public Animator  bossAnimator;
    public AudioClip fireSound;
    private AudioSource audioSource;
    private void Awake() => audioSource = GetComponent<AudioSource>();

    protected override IEnumerator DoAttack(BossController boss)
    {
        Debug.Log("BossAnt: AttackFireCross");
        bossAnimator?.SetTrigger("FireCross");
        audioSource?.PlayOneShot(fireSound);
        yield return new WaitForSeconds(0.4f);

        if (fireBulletPrefab == null) yield break;

        GameObject[] bullets = new GameObject[4];
        float[]      angles  = { 0f, 90f, 180f, 270f };

        for (int i = 0; i < 4; i++)
        {
            float   rad    = angles[i] * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * startRadius;
            bullets[i] = Instantiate(fireBulletPrefab,
                                     boss.transform.position + offset,
                                     Quaternion.identity);
            FireBullet fb = bullets[i].GetComponent<FireBullet>();
            if (fb != null) fb.damage = damage;
        }

        float elapsed       = 0f;
        float currentAngle  = 0f;
        float currentRadius = startRadius;

        while (elapsed < crossDuration)
        {
            currentAngle  += rotationSpeed * Time.deltaTime;
            currentRadius += expansionSpeed * Time.deltaTime;

            for (int i = 0; i < 4; i++)
            {
                if (bullets[i] == null) continue;
                float   rad    = (angles[i] + currentAngle) * Mathf.Deg2Rad;
                Vector3 newPos = boss.transform.position
                    + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * currentRadius;

                bullets[i].transform.position = newPos;
                bullets[i].transform.right    = (newPos - boss.transform.position).normalized;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var b in bullets)
            if (b != null) Destroy(b);

        yield return new WaitForSeconds(0.3f);
    }
}
