using System.Collections;
using UnityEngine;

/// <summary>
/// Jet d'acide parabolique + flaque persistante au sol.
/// </summary>
public class AttackAcid : BaseAttack
{
    [Header("Projectile")]
    public GameObject acidProjectilePrefab;
    public float      projectileSpeed        = 8f;
    public float      projectileGravityScale = 1.2f;

    [Header("Flaque")]
    public GameObject acidPoolPrefab;
    public float      poolDuration     = 4f;
    public float      poolDamagePerSec = 5f;

    public Animator  bossAnimator;
    public Transform spawnPoint;

    protected override IEnumerator DoAttack(BossController boss)
    {
        Debug.Log("BossAnt: AttackAcid");
        bossAnimator?.SetTrigger("Acid");
        yield return new WaitForSeconds(0.25f);

        if (acidProjectilePrefab == null) yield break;

        Vector2 dir      = boss.DirectionToPlayer();
        Vector3 spawnPos = spawnPoint != null
            ? spawnPoint.position
            : boss.transform.position + (Vector3)dir * 0.8f;

        GameObject  proj   = Instantiate(acidProjectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.gravityScale   = projectileGravityScale;
            projRb.linearVelocity = dir * projectileSpeed;
        }

        AcidProjectile acidComp = proj.GetComponent<AcidProjectile>();
        if (acidComp != null)
        {
            acidComp.poolPrefab       = acidPoolPrefab;
            acidComp.poolDuration     = poolDuration;
            acidComp.poolDamagePerSec = poolDamagePerSec;
        }

        yield return new WaitForSeconds(0.6f);
    }
}
