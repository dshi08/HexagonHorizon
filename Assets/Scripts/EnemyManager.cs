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
    

    [Header("States")]
    public bool isDead = false;
    public bool canAttack = true;
    public float attackCooldown = 2f;

    // public delegate void EnemyEvent(EnemyManager enemy);
    // public static event EnemyEvent OnEnemyDeath;

    void Awake()
    {
        _currentHealth = maxHealth;
        _enemyMovement = GetComponent<EnemyMovement>();
        // _animator = GetComponent<Animator>();
        
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;

        HandleAIBehavior();
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        // _animator?.SetTrigger("Hurt");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        // _animator?.SetTrigger("Die");
        
        // Disable components
        // if (TryGetComponent<Collider>(out var collider))
        //     collider.enabled = false;
        
        // _enemyMovement?.StopMovement();

        // Notify listeners (exp stuff)
        // OnEnemyDeath?.Invoke(this);

        // Visual/audio effects
        // Instantiate(deathEffect, transform.position, Quaternion.identity);
        // Destroy(gameObject, 2f); // Delay for death animation
    }
    private void HandleAIBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // if (distanceToPlayer <= attackRange && canAttack)
        {
            // StartCoroutine(AttackPlayer());
        }
        // else if (distanceToPlayer > attackRange)
        {
            // _enemyMovement?.MoveToward(player.position);
        }
    }

    // IEnumerator AttackPlayer()
    // {
    //     canAttack = false;
    //     // _animator?.SetTrigger("Attack");
        
    //     // Damage logic (called via animation event)
    //     // yield return new WaitForSeconds(attackCooldown);
    //     // canAttack = true;
    // }

    // Called from animation event
    public void DealDamage()
    {
        // if (Vector3.Distance(transform.position, player.position) <= attackRange)
        // {
        //     player.GetComponent<PlayerHealth>().TakeDamage(damage);
        // }
    }

    // ========== DEBUG ==========
    void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
