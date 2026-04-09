using System.Collections;
using UnityEngine;

/// <summary>
/// Phase 2 — Nuage de phéromones qui ralentit le joueur.
/// Compatible avec PlayerController (modifie moveSpeed directement).
/// </summary>
public class AttackPheromones : BaseAttack
{
    [Header("Phéromones")]
    public float duration       = 5f;
    public float slowMultiplier = 0.4f; // vitesse × 0.4

    public GameObject pheromonesVFX;
    public Animator   bossAnimator;

    protected override IEnumerator DoAttack(BossController boss)
    {
        bossAnimator?.SetTrigger("Pheromones");
        yield return new WaitForSeconds(0.3f);

        GameObject vfx = null;
        if (pheromonesVFX != null)
            vfx = Instantiate(pheromonesVFX, boss.transform.position, Quaternion.identity);

        // Ralentit le PlayerController du joueur
        PlayerController pc = boss.player?.GetComponent<PlayerController>();
        if (pc != null)
        {
            float original = pc.moveSpeed;
            pc.moveSpeed   = original * slowMultiplier;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (vfx != null) vfx.transform.position = boss.transform.position;
                elapsed += Time.deltaTime;
                yield return null;
            }

            pc.moveSpeed = original; // restaure la vitesse
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }

        if (vfx != null) Destroy(vfx);
        yield return new WaitForSeconds(0.2f);
    }
}
