using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    [SerializeField] private int _currentHealth;
    // public int damage = 15;
    // public float moveSpeed = 3f;
    // public float attackRange = 1.5f;
    // public int scoreValue = 100;
    
    [Header("References")]
    public Transform player;
    // public GameObject deathEffect;
    private EnemyMovement _enemyMovement;
    // private Animator _animator;
}
