using System.Collections;
using UnityEngine;

/// <summary>
/// Charge frontale rapide avec traînée de braises.
/// </summary>
public class AttackCharge : BaseAttack
{
    [Header("Charge")]
    public float damage         = 20f;
    public float chargeSpeed    = 12f;
    public float chargeDuration = 0.5f;

    [Header("Braises")]
    public GameObject emberPrefab;
    public float      emberSpread = 0.4f;

    public Rigidbody2D bossRb;
    public Animator    bossAnimator;

    protected override IEnumerator DoAttack(BossController boss)
    {
        bossAnimator?.SetTrigger("Charge");
        yield return new WaitForSeconds(0.4f);

        Vector2 chargeDir = boss.DirectionToPlayer();
        float   elapsed   = 0f;

        boss.GetComponent<ChargeHitbox>()?.Activate(damage, chargeDuration);

        while (elapsed < chargeDuration)
        {
            bossRb.linearVelocity = chargeDir * chargeSpeed;

            if (emberPrefab != null && Random.value > 0.6f)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-emberSpread, emberSpread),
                    Random.Range(-emberSpread, emberSpread), 0);
                Instantiate(emberPrefab, boss.transform.position + offset, Quaternion.identity);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        bossRb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
    }
}
