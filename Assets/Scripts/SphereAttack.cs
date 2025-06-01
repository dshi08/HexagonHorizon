using UnityEngine;
public class SphereAttack : AttackBase
{
    public int baseDamage = 10;
    public float radius = 1.5f;
    public KeyCode assignedKey = KeyCode.Alpha1;
    public string displayName = "Orb Blast";

    public override void Execute()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var target))
                target.TakeDamage(damage);
        }
    }

    public override KeyCode hotkey => assignedKey;
    public override string attackName => displayName;
    public override int damage => baseDamage;
}