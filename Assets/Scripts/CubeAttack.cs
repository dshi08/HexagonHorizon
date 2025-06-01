using UnityEngine;

public class CubeAttack : AttackBase
{
    [Header("Cube Attack Settings")]
    public int baseDamage = 20; // Double damage (10 x 2)
    public float cubeSize = 2f; // World units (length/width/height)
    public KeyCode assignedKey = KeyCode.Alpha3;
    public string displayName = "Crushing Cube";
    public GameObject cubeEffectPrefab;

    public override void Execute()
    {
        // Spawn visual effect
        if (cubeEffectPrefab != null)
        {
            GameObject cube = Instantiate(
                cubeEffectPrefab,
                transform.position + transform.forward * (cubeSize/2),
                transform.rotation
            );
            Destroy(cube, 0.5f);
        }

        // Damage calculation
        Vector3 cubeCenter = transform.position + transform.forward * (cubeSize/2);
        Collider[] hits = Physics.OverlapBox(
            cubeCenter,
            Vector3.one * (cubeSize/2),
            transform.rotation
        );

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
            }
        }
    }

    public override KeyCode hotkey => assignedKey;
    public override string attackName => displayName;
    public override int damage => baseDamage;

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Vector3 center = transform.position + transform.forward * (cubeSize/2);
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * cubeSize);
    }
}