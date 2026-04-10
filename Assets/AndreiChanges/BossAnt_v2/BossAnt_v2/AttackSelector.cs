using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Choisit l'attaque à lancer par tirage pondéré selon la phase courante.
/// </summary>
public class AttackSelector : MonoBehaviour
{
    [Header("Attaques — assigne dans l'Inspector")]
    public AttackStinger    stingerAttack;
    public AttackAcid       acidAttack;
    public AttackCharge     chargeAttack;
    public AttackSpawnMinions spawnAttack;
    public AttackLarvaBomb  larvaBombAttack;
    public AttackPheromones pheromonesAttack;
    public AttackFireCross  fireCrossAttack;
    public AttackInvasion   invasionAttack;

    [Header("Délai entre attaques")]
    public float minDelay = 0.8f;
    public float maxDelay = 1.4f;

    private PhaseManager phaseManager;
    private void Awake() => phaseManager = GetComponent<PhaseManager>();

    public IEnumerator SelectAndExecute(BossController boss)
    {
        boss.IsAttacking = true;
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

        BaseAttack chosen = PickAttack(phaseManager.CurrentPhase);
        if (chosen != null && chosen.IsReady())
        {
            boss.FacePlayer();
            yield return chosen.Execute(boss);
        }

        boss.IsAttacking = false;
    }

    private BaseAttack PickAttack(PhaseManager.Phase phase)
    {
        var pool = new List<(BaseAttack atk, int weight)>();

        switch (phase)
        {
            case PhaseManager.Phase.One:
                pool.Add((stingerAttack, 30));
                pool.Add((acidAttack,    30));
                pool.Add((chargeAttack,  30));
                pool.Add((spawnAttack,   10));
                break;

            case PhaseManager.Phase.Two:
                pool.Add((stingerAttack,   15));
                pool.Add((acidAttack,      20));
                pool.Add((chargeAttack,    20));
                pool.Add((spawnAttack,     10));
                pool.Add((larvaBombAttack, 25));
                pool.Add((pheromonesAttack,10));
                break;

            case PhaseManager.Phase.Three:
                pool.Add((chargeAttack,  10));
                pool.Add((fireCrossAttack,35));
                pool.Add((invasionAttack, 35));
                pool.Add((acidAttack,     20));
                break;
        }

        return WeightedRandom(pool);
    }

    private BaseAttack WeightedRandom(List<(BaseAttack atk, int weight)> pool)
    {
        int total = 0;
        foreach (var p in pool) total += p.weight;

        int roll = Random.Range(0, total);
        int cum  = 0;
        foreach (var p in pool)
        {
            cum += p.weight;
            if (roll < cum && p.atk != null && p.atk.IsReady())
                return p.atk;
        }
        return stingerAttack;
    }
}
