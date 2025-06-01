
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject enemyManagerObject;
    private EnemyManager enemyManager;
    public List<AttackBase> attacks;

    void Start()
    {
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();
	}

	public void Attack(Vector2 origin, Vector2 target, int attackIndex)
    {
        AttackBase attack = attacks[attackIndex];
        List<Vector2> hitTiles = attack.GetHitTiles(origin, target);

        foreach (Vector2 tile in hitTiles)
        {
            enemyManager.DamageEnemy(tile, attack.damage);
        }
    }
}
