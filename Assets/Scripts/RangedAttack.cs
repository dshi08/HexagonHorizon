using System.Collections.Generic;
using UnityEngine;
public class RangedAttack : AttackBase
{
    public GameObject cannonballPrefab;
    public override List<Vector2> HitTiles(Vector2 origin, Vector2 target)
    {
        Vector3 startPos = HexPos.AxialToPositionStatic((int)origin.x, (int)origin.y);
        Vector3 endPos = HexPos.AxialToPositionStatic((int)target.x, (int)target.y);
        GameObject cannonball = Instantiate(cannonballPrefab, startPos, Quaternion.identity);
        Cannonball cannonballScript = cannonball.GetComponent<Cannonball>();
        cannonballScript.Shoot(startPos, endPos);
        
        return new List<Vector2> { target };
    }
    public override int damage => 9;
}