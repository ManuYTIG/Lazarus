using System.Collections;
using UnityEngine;

/// <summary>
/// Phase 2 — Bombes de larves en éventail qui s'éclosent après un délai.
/// </summary>
public class AttackLarvaBomb : BaseAttack
{
    [Header("Larves")]
    public GameObject larvaPrefab;
    public int        larvaCount  = 5;
    public float      launchSpeed = 6f;
    public float      hatchDelay  = 3f;
    public float      spread      = 60f;

    public Animator bossAnimator;

    protected override IEnumerator DoAttack(BossController boss)
    {
        bossAnimator?.SetTrigger("LarvaBomb");
        yield return new WaitForSeconds(0.35f);

        if (larvaPrefab == null) yield break;

        Vector2 baseDir   = boss.DirectionToPlayer();
        float   baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < larvaCount; i++)
        {
            float   angle     = baseAngle + Random.Range(-spread / 2f, spread / 2f);
            Vector2 launchDir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject  larva   = Instantiate(larvaPrefab, boss.transform.position, Quaternion.identity);
            Rigidbody2D larvaRb = larva.GetComponent<Rigidbody2D>();
            if (larvaRb != null) larvaRb.linearVelocity = launchDir * launchSpeed;

            LarvaEgg egg = larva.GetComponent<LarvaEgg>();
            if (egg != null) egg.hatchDelay = hatchDelay;

            yield return new WaitForSeconds(0.08f);
        }

        yield return new WaitForSeconds(0.5f);
    }
}
