
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    // All possible attack types
    public enum AttackType { Sphere, Cone, Cube } // Think of more(One that can curve onto nearest enemy)

    [System.Serializable]
    public class AttackUnlock
    {
        public AttackType type;
        public bool isUnlocked;
        public int requiredLevel;
    }

    [Header("Attacks")]
    public List<AttackUnlock> attackProgression = new List<AttackUnlock>()
    {
        new AttackUnlock { type = AttackType.Sphere, isUnlocked = true, requiredLevel = 0 },
        new AttackUnlock { type = AttackType.Cone, isUnlocked = false, requiredLevel = 3 },
        new AttackUnlock { type = AttackType.Cube, isUnlocked = false, requiredLevel = 5 }
    };

    [Header("References")]
    public SphereAttack sphereAttack;
    public ConeAttack coneAttack;
    public CubeAttack cubeAttack;

    private Dictionary<AttackType, AttackBase> attackMap;

    void Awake()
    {
        // Map attack types to their implementations
        attackMap = new Dictionary<AttackType, AttackBase>()
        {
            { AttackType.Sphere, sphereAttack },
            { AttackType.Cone, coneAttack },
            { AttackType.Cube, cubeAttack }
        };
    }

    public void UnlockAttack(AttackType type)
    {
        AttackUnlock unlock = attackProgression.Find(a => a.type == type);
        if (unlock != null) unlock.isUnlocked = true;
    }

    public bool TryGetAttack(AttackType type, out AttackBase attack)
    {
        AttackUnlock unlock = attackProgression.Find(a => a.type == type);
        if (unlock != null && unlock.isUnlocked)
        {
            attack = attackMap[type];
            return true;
        }
        attack = null;
        return false;
    }
}
