using UnityEngine;

public class LightStickItem : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingRange = 1.2f;
    public float swingAngle = 60f;
    public float swingDuration = 0.15f;
    public float damage = 1f;
    public string[] immuneTags = { "Player", "Boss" }; // Boss ne reçoit pas de dégâts

    private bool isSwinging = false;

    public void TrySwing(Vector3 playerPos, Vector3 mouseWorldPos)
    {
        if (isSwinging) return;
        StartCoroutine(SwingRoutine(playerPos, mouseWorldPos));
    }

    private System.Collections.IEnumerator SwingRoutine(Vector3 playerPos, Vector3 mouseWorldPos)
    {
        isSwinging = true;

        Vector2 direction = (mouseWorldPos - playerPos).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Arc de -swingAngle/2 à +swingAngle/2 autour de la direction souris
        int steps = 5;
        float halfAngle = swingAngle / 2f;

        for (int i = 0; i <= steps; i++)
        {

            float t = (float)i / steps;
            float currentAngle = (baseAngle - halfAngle + swingAngle * t) * Mathf.Deg2Rad;
            Vector2 checkDir = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));

            // Raycast dans chaque direction de l'arc
            RaycastHit2D hit = Physics2D.Raycast(playerPos, checkDir, swingRange, ~LayerMask.GetMask("Player"));
Debug.DrawRay(playerPos, checkDir * swingRange, Color.yellow, 0.2f);
Debug.Log($"Ray hit: {(hit.collider != null ? hit.collider.name : "nothing")}");

            if (hit.collider != null)
            {
                bool immune = false;
                foreach (string tag in immuneTags)
                    if (hit.collider.CompareTag(tag)) { immune = true; break; }

                if (!immune && hit.collider.TryGetComponent(out Health h))
                    h.RemoveHealth(damage);
            }
        }

        yield return new WaitForSeconds(swingDuration);
        isSwinging = false;
    }
}
