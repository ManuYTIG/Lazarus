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

    [Header("Knockback")]
    public float knockbackForce    = 18f;
    public float knockbackDuration = 0.25f;

    [Header("Braises")]
    public GameObject  emberPrefab;
    public float       emberSpread = 0.4f;
    public Rigidbody2D bossRb;
    public Animator    bossAnimator;

    private bool IsAttacking = false;

    // Stored so OnCollisionEnter2D knows the current charge direction
    private Vector2 _chargeDir;

    protected override IEnumerator DoAttack(BossController boss)
    {
        IsAttacking = true;
        Debug.Log("BossAnt: AttackCharge");
        bossAnimator?.SetTrigger("Charge");
        yield return new WaitForSeconds(0.4f);

        _chargeDir = boss.DirectionToPlayer();
        float elapsed = 0f;

        boss.GetComponent<ChargeHitbox>()?.Activate(damage, chargeDuration);

        while (elapsed < chargeDuration)
        {
            bossRb.linearVelocity = _chargeDir * chargeSpeed;

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
        IsAttacking = false;
        yield return new WaitForSeconds(0.4f);
    }

    // Called by ChargeHitbox OR OnCollisionEnter2D
    private void ApplyKnockback(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        StartCoroutine(KnockbackRoutine(playerRb));
    }

    private IEnumerator KnockbackRoutine(Rigidbody2D playerRb)
    {
        // Optionally disable player control here via a PlayerController method
        // e.g. playerRb.GetComponent<PlayerController>()?.SetInputEnabled(false);

        playerRb.linearVelocity = Vector2.zero;
        playerRb.AddForce(_chargeDir * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        // Optionally re-enable player control here
        // e.g. playerRb.GetComponent<PlayerController>()?.SetInputEnabled(true);

        // Dampen residual knockback velocity
        playerRb.linearVelocity *= 0.2f;
    }

    // Fixed: was lowercase 'o' — Unity would never call it
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && IsAttacking)
        {
            Health h = col.gameObject.GetComponent<Health>();
            if (h != null) h.RemoveHealth(damage);

            ApplyKnockback(col.gameObject);
        }
    }
}