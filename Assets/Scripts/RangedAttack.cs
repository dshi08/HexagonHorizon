using System.Collections.Generic;
using UnityEngine;
public class RangedAttack : AttackBase
{
    public override List<Vector2> GetHitTiles(Vector2 origin, Vector2 target)
    {
        return new List<Vector2>{ target };
    }
    public override int damage => 9;
}