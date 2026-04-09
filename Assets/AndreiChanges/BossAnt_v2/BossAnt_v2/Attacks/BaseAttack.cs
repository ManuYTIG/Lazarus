using System.Collections;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    [Header("Cooldown (secondes)")]
    public float cooldown = 3f;
    protected float lastUsedTime = -999f;

    public bool IsReady() => Time.time >= lastUsedTime + cooldown;

    public IEnumerator Execute(BossController boss)
    {
        lastUsedTime = Time.time;
        yield return DoAttack(boss);
    }

    protected abstract IEnumerator DoAttack(BossController boss);
}
