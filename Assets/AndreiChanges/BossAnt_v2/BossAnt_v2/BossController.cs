using System.Collections;
using UnityEngine;

/// <summary>
/// Contrôleur principal de la Reine des Fourmis de Feu.
/// Composants requis : Rigidbody2D, Animator, SpriteRenderer, Health,
///                     PhaseManager, AttackSelector, + un script par attaque
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class BossController : MonoBehaviour
{
    [Header("Comportement")]
    public float detectionRange = 12f;

    [Header("Références")]
    public Transform  player;
    public GameObject deathVFX;

    [Header("Sprites")]
    public RuntimeAnimatorController flyingAnimator; // animator avec les 8 frames de vol
    public Sprite                    groundSprite;   // sprite unique quand posée (phase 3)

    [HideInInspector] public Rigidbody2D    rb;
    [HideInInspector] public Animator       anim;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Health         health;

    private PhaseManager   phaseManager;
    private AttackSelector attackSelector;

    public bool IsDead      { get; private set; }
    public bool IsAttacking { get; set; }

    public bool PlayerInRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    private void Awake()
    {
        rb             = GetComponent<Rigidbody2D>();
        anim           = GetComponent<Animator>();
        sr             = GetComponent<SpriteRenderer>();
        health         = GetComponent<Health>();
        phaseManager   = GetComponent<PhaseManager>();
        attackSelector = GetComponent<AttackSelector>();
    }

    private void Start()
    {
        StartCoroutine(WatchHP());
        StartCoroutine(BossLoop());
    }

    // Surveille les HP pour déclencher les transitions de phase
    private IEnumerator WatchHP()
    {
        while (!IsDead)
        {
            float ratio = health.health / health.maxHealth;
            phaseManager.CheckPhaseTransition(ratio);
            if (health.health <= 0) Die();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator BossLoop()
    {
        yield return new WaitForSeconds(1.5f);

        while (!IsDead)
        {
            if (!IsAttacking)
            {
                // NEW: only act if player is in range
                if (PlayerInRange())
                {
                    FacePlayer();
                    yield return attackSelector.SelectAndExecute(this);
                }
            }
            yield return null;
        }
    }

    // Appelé par PhaseManager quand on arrive en phase 3
    public void SwitchToGroundSprite()
    {
        anim.enabled = false; // arrête l'animation de vol
        if (groundSprite != null)
            sr.sprite = groundSprite;
    }

    private void Die()
    {
        if (IsDead) return;
        IsDead = true;
        StopAllCoroutines();
        anim.SetTrigger("Die");
        if (deathVFX != null)
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        BroadcastMessage("OnQueenDead", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject, 2f);
    }

    public void FacePlayer()
    {
        if (player == null) return;
        sr.flipX = player.position.x < transform.position.x;
    }

    public Vector2 DirectionToPlayer()
    {
        if (player == null) return Vector2.right;
        return (player.position - transform.position).normalized;
    }

    // Direction opposée au joueur — utilisée par l'attaque de dard
    public Vector2 DirectionAwayFromPlayer()
    {
        return -DirectionToPlayer();
    }
}
