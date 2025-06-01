using System.Collections.Generic;
using UnityEngine;
public class LineAttack : AttackBase
{
    public override List<Vector2> HitTiles(Vector2 origin, Vector2 target)
    {
        return new List<Vector2>
        {
            new Vector2(target.x + 1, target.y),     // East
            new Vector2(target.x + 1, target.y - 1), // Northeast
            new Vector2(target.x, target.y - 1),     // Northwest
            new Vector2(target.x - 1, target.y),     // West
            new Vector2(target.x - 1, target.y + 1), // Southwest
            new Vector2(target.x, target.y + 1) 
        };
    }
    public override int damage => 4;
}