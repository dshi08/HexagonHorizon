using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UE = UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    public int enemyCount = 5;
    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    private HexPos playerHex;

    void Start()
    {
        SpawnEnemies();
        playerHex = player.GetComponent<HexPos>();
    }

    public void Move()
    {
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.ChasePlayer(playerHex);
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            HexPos hexPos = enemyObj.GetComponent<HexPos>();
            int q = UE.Random.Range(-5, 5);
            int magnitude = 5 - Math.Abs(q);
            int r = UE.Random.Range(-magnitude, magnitude);
            hexPos.SetPos(q, r);
            enemyObj.transform.position = hexPos.position;
            enemies.Add(enemyObj.GetComponent<EnemyMovement>());
        }
    }
}
