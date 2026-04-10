using System.Collections;
using UnityEngine;

/// <summary>
/// Attaque de dard : la reine pivote et pique DERRIÈRE elle.
/// Le dard sort dans la direction opposée au joueur.
/// </summary>
public class AttackStinger : BaseAttack
{
    [Header("Dard")]
    public float damage    = 20f;
    public float range     = 2f;    // portée du dard
    public float hitDelay  = 0.35f; // temps avant le hit (sync avec l'anim)

    [Header("Visuel du dard (optionnel)")]
    public GameObject stingerVFX;   // prefab flash/impact au bout du dard
    public float      vfxDuration = 0.2f;

    [Header("Références")]
    public Animator  bossAnimator;
    public AudioClip stingSound;
    private AudioSource audioSource;
    private void Awake() => audioSource = GetComponent<AudioSource>();

    protected override IEnumerator DoAttack(BossController boss)
    {
        bossAnimator?.SetTrigger("Stinger");
        audioSource?.PlayOneShot(stingSound);

        yield return new WaitForSeconds(hitDelay);

        // Le dard pointe dans la direction OPPOSÉE au joueur (dans le cul)
        Vector2 stingDir = boss.DirectionAwayFromPlayer();
        Vector2 stingTip = (Vector2)boss.transform.position + stingDir * range;

        // VFX à la pointe du dard
        if (stingerVFX != null)
        {
            GameObject vfx = Instantiate(stingerVFX,
                                         new Vector3(stingTip.x, stingTip.y, 0),
                                         Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }

        // Détecte le joueur dans la zone du dard
        Collider2D[] hits = Physics2D.OverlapCircleAll(stingTip, 0.6f);
        foreach (var col in hits)
        {
            if(col.gameObject.CompareTag("Player"))
            {
                Health h = col.GetComponent<Health>();
                if (h != null) h.RemoveHealth(damage);
            }       
        }

        yield return new WaitForSeconds(0.5f);
    }

    // Affiche la zone de hit dans l'éditeur Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.5f);
        // Montre la portée max du dard derrière le boss
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
