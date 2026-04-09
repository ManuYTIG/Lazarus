using UnityEngine;
using System.Collections;

/// <summary>
/// Phase 1 et 2 : la reine vole (animator actif, 8 frames en rotation).
/// Phase 3      : elle atterrit (animator désactivé, sprite unique).
/// </summary>
public class PhaseManager : MonoBehaviour
{
    public enum Phase { One, Two, Three }

    [Header("Seuils HP (ratio 0-1)")]
    public float phase2Threshold = 0.66f;
    public float phase3Threshold = 0.33f;

    [Header("VFX de transition")]
    public GameObject transitionVFX;
    public float      screenShakeDuration  = 0.4f;
    public float      screenShakeMagnitude = 0.3f;

    public Phase CurrentPhase { get; private set; } = Phase.One;

    private BossController boss;
    private bool           transitioning = false;

    private void Awake() => boss = GetComponent<BossController>();

    public void CheckPhaseTransition(float hpRatio)
    {
        if (transitioning) return;

        if (CurrentPhase == Phase.One && hpRatio <= phase2Threshold)
            StartCoroutine(TransitionTo(Phase.Two));
        else if (CurrentPhase == Phase.Two && hpRatio <= phase3Threshold)
            StartCoroutine(TransitionTo(Phase.Three));
    }

    private IEnumerator TransitionTo(Phase newPhase)
    {
        transitioning    = true;
        CurrentPhase     = newPhase;
        boss.IsAttacking = true;

        // Phase 3 seulement : la reine se pose et change de sprite
        if (newPhase == Phase.Three)
            boss.SwitchToGroundSprite();

        // Phase 2 : elle continue de voler, juste une petite pause de rage
        if (transitionVFX != null)
            Instantiate(transitionVFX, transform.position, Quaternion.identity);

        yield return StartCoroutine(ScreenShake(screenShakeDuration, screenShakeMagnitude));
        yield return new WaitForSeconds(0.6f);

        boss.IsAttacking = false;
        transitioning    = false;

        Debug.Log($"[BossAnt] Transition → {newPhase}");
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Camera  cam         = Camera.main;
        Vector3 originalPos = cam.transform.localPosition;
        float   elapsed     = 0f;

        while (elapsed < duration)
        {
            cam.transform.localPosition = originalPos + new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = originalPos;
    }
}
