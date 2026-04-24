using UnityEngine;
using System.Collections;
public class LightStickItem : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingRange = 1.2f;
    public float swingAngle = 60f;
    public float swingDuration = 0.15f;
    public float damage = 0.5f;
    public string[] immuneTags = { "Player", "Boss" }; // Boss ne reçoit pas de dégâts
    private Animator holderAnimator;
    private float lastSwingTime = 0f;
    private const float swingCooldown = 0.2f; // 2 swings per second


    private void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Holder");
        if(obj != null )
            holderAnimator = obj.GetComponent<Animator>();
    }

    public void TrySwing(Vector3 playerPos, Vector3 mouseWorldPos)
    {
        // Cooldown: 2 swings per second
        if (Time.time - lastSwingTime < swingCooldown)
            return;

        lastSwingTime = Time.time;

        // Force animation to restart from frame 0
        holderAnimator?.Play("Swing", 0, 0f);

        // Start a new damage sweep (multiple allowed)
        StartCoroutine(SwingRoutine(playerPos, mouseWorldPos));
    }




    private IEnumerator SwingRoutine(Vector3 playerPos, Vector3 mouseWorldPos)
    {
        Vector2 direction = (mouseWorldPos - playerPos).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        yield return new WaitForSeconds(0.05f);

        int steps = 5;
        float halfAngle = swingAngle / 2f;

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            float currentAngle = (baseAngle - halfAngle + swingAngle * t) * Mathf.Deg2Rad;
            Vector2 checkDir = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
            int mask = ~LayerMask.GetMask("Player");

            RaycastHit2D hit = Physics2D.Raycast(playerPos, checkDir, swingRange, mask);
            Debug.DrawRay(playerPos, checkDir * swingRange, Color.yellow, 0.2f);

            if (hit.collider != null)
            {
                bool immune = false;
                foreach (string tag in immuneTags)
                    if (hit.collider.CompareTag(tag)) { immune = true; break; }

                if (!immune && hit.collider.TryGetComponent(out Health h))
                {
                    h.RemoveHealth(damage);
                    break;
                }
            }
        }

        yield return new WaitForSeconds(swingDuration);
    }

}
