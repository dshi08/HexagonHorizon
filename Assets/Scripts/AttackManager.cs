
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public List<AttackBase> attacks;

    public void Attack(Vector2 origin, Vector2 target, int attackIndex)
    {
        AttackBase attack = attacks[attackIndex];
        List<Vector2> hitTiles = attack.GetHitTiles(origin, target);

        foreach (Vector2 tile in hitTiles)
        {
            Debug.Log($"ATTACKING {tile}");
        }
    }
}
