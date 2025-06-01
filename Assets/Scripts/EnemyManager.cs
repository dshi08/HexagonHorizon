using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UE = UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject gridManagerObject;
    private GridManager gridManager;
    public GameObject player;
    public GameObject healthBar;
    private Health playerHealth;
    public int enemyCount = 5;
    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    private HexPos playerHex;
    private List<EnemyMovement> toDestroy = new List<EnemyMovement>();
    private PlayerXP playerXP;
    void Start()
    {
        playerHex = player.GetComponent<HexPos>();
        playerHealth = healthBar.GetComponent<Health>();
        gridManager = gridManagerObject.GetComponent<GridManager>();
        SpawnEnemies();
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
            gridManager.EnterHex(q, r);

            enemyObj.transform.position = hexPos.position;

            EnemyMovement enemy = enemyObj.GetComponent<EnemyMovement>();
            enemy.playerHealth = playerHealth;
            enemy.gridManager = gridManager;
            enemies.Add(enemy);
        }
    }

    public void DamageEnemy(Vector2 hex, int damage)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyMovement enemy = enemies[i];
            if (enemy.hexPos.GetPos() == hex)
            {
                enemy.health -= damage;
                if (enemy.health <= 0)
                {
                    toDestroy.Add(enemy);
                    Invoke("DestroyEnemies", 0.5f);
                }
                return;
            }
        }
    }

    public void EnemyKilled(EnemyMovement deadEnemy)
    {
        Debug.Log($"Enemy {deadEnemy.gameObject.name} has been defeated!");

        // Grant XP to the player
        if (playerXP != null)
        {
            playerXP.GainXP(deadEnemy.xpValue); // Uses the xpValue from the EnemyMovement script
        }
        else
        {
            Debug.LogWarning("PlayerXP reference is null when trying to grant XP.");
        }
        // Remove the enemy from the active list
        enemies.Remove(deadEnemy);
        // Free up the hex on the grid
        gridManager.LeaveHex(deadEnemy.hexPos.q, deadEnemy.hexPos.r);
        // Destroy the GameObject
        Destroy(deadEnemy.gameObject);
    }
    void DestroyEnemies()
    {
        foreach (EnemyMovement enemy in toDestroy)
        {
            enemies.Remove(enemy);
            gridManager.LeaveHex(enemy.hexPos.q, enemy.hexPos.r);
            Destroy(enemy.gameObject);
        }
        toDestroy.Clear();
    }
}
