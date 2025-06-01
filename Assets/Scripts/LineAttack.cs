using System;
using System.Collections.Generic;
using UnityEngine;
public class LineAttack : AttackBase
{
    public GameObject harpoonPrefab;
    public override List<Vector2> HitTiles(Vector2 origin, Vector2 target)
    {
        Vector3 startPos = HexPos.AxialToPositionStatic((int)origin.x, (int)origin.y);
        Vector3 endPos = HexPos.AxialToPositionStatic((int)target.x, (int)target.y);
        Vector3 direction = (endPos - startPos).normalized;
        GameObject harpoon = Instantiate(harpoonPrefab, origin, Quaternion.identity);
        // face the harpoon towards the target
        harpoon.transform.LookAt(endPos);
        
        harpoon.GetComponent<Cannonball>().Shoot(startPos, endPos + direction * 50f, 2f);

        List<Vector2> hitTiles = new List<Vector2>();

        float x = origin.x;
        float y = origin.y;
        for (int i = 0; i < 20; i++)
        {
            hitTiles.Add(new Vector2((float)Math.Round(x), (float)Math.Round(y)));

            // Move in the direction of the target
            x += target.x - origin.x;
            y += target.y - origin.y;
        }

        return hitTiles;
    }
    public override int damage => 6;
}