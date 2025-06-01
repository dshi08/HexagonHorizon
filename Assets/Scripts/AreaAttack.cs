using System.Collections.Generic;
using UnityEngine;
public class AreaAttack : AttackBase
{
    public GameObject gridManagerObject;
    private GridManager gridManager;
    private List<Vector2> hitTiles;

    private void Start()
    {
        gridManager = gridManagerObject.GetComponent<GridManager>();
    }

    public override List<Vector2> HitTiles(Vector2 origin, Vector2 target)
    {
        hitTiles = new List<Vector2>{
            new Vector2(origin.x + 1, origin.y),     // East
            new Vector2(origin.x + 1, origin.y - 1), // Northeast
            new Vector2(origin.x, origin.y - 1),     // Northwest
            new Vector2(origin.x - 1, origin.y),     // West
            new Vector2(origin.x - 1, origin.y + 1), // Southwest
            new Vector2(origin.x, origin.y + 1)
        };

        foreach (Vector2 tile in hitTiles)
        {
            string tileKey = $"{(int)tile.x},{(int)tile.y}";
            if (!gridManager.hexes.ContainsKey(tileKey)) continue;

            GameObject hex = gridManager.hexes[tileKey];
            hex.GetComponent<Animator>().SetBool("Flipped", false);
        }

        Invoke("Unflip", 0.5f);

        return hitTiles;
    }

    public void Unflip()
    {
        foreach (Vector2 tile in hitTiles)
        {
            string tileKey = $"{(int)tile.x},{(int)tile.y}";
            if (!gridManager.hexes.ContainsKey(tileKey)) continue;

            GameObject hex = gridManager.hexes[tileKey];
            hex.GetComponent<Animator>().SetBool("Flipped", true);
        }
    }

    public override int damage => 4;
}