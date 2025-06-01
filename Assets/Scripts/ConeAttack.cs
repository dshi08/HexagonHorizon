using UnityEngine;

public class ConeAttack : AttackBase
{
    [Header("Piercing Cone Settings")]
    public int baseDamage = 10;
    public float radius = 0.5f; // Width of the piercing beam
    public float maxDistance = 5f; // How far it penetrates
    public int maxPierceCount = 3; // Max enemies it can pierce through
    public KeyCode assignedKey = KeyCode.Alpha2;
    public string displayName = "Piercing Beam";

    public override void Execute()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // Perform a capsule cast to detect all enemies in the piercing path
        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            radius,
            direction,
            maxDistance
        );

        int enemiesPierced = 0;
        
        // Sort hits by distance to ensure proper piercing order
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (enemiesPierced >= maxPierceCount) break;

            if (hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
                enemiesPierced++;
            }
        }
    }

    public override KeyCode hotkey => assignedKey;
    public override string attackName => displayName;
    public override int damage => baseDamage;

    // Debug Visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * maxDistance;
        
        // Draw the piercing "beam" as a cylinder
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
    }
}