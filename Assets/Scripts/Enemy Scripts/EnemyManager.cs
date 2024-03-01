using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour, IDamageable
{
    // Health options
    [Header("Health Options")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private EnemyHealthBar enemyHealthBar;

    // Loot options
    [Header("Loot Options")]
    [SerializeField] private int minimumLootDrop = 5;
    [SerializeField] private int maximumLootDrop = 10;

    // Particle Effect
    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem deathEffect;

    // Event for enemy death
    [HideInInspector]
    public UnityEvent enemyDeath;

    // References to other scripts
    private PlayerManager playerManager;
    private EnemySpawn enemySpawn;

    // Current health and loot drop
    private int currentHealth;
    private int lootDrop;

    private void Awake()
    {
        // Get reference to enemy health bar
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        deathEffect = GetComponentInChildren<ParticleSystem>();

        // Randomize loot drop
        lootDrop = Random.Range(minimumLootDrop, maximumLootDrop);

        // Get references to other scripts
        enemySpawn = EnemySpawn.Instance;
        playerManager = PlayerManager.Instance;

        // Add listener for enemy death event
        enemyDeath.AddListener(() => playerManager.IncrementMoney(lootDrop));

        // Initialize current health and set max health for health bar
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        // Update enemy health bar
        enemyHealthBar.SetHealth(currentHealth);
    }

    // Method to handle taking damage
    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;

        // Check if enemy is dead
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            enemyDeath.Invoke();

            // Plays death effect
            deathEffect.Play();

            SoundManager.Instance.Play("Boar Squeel");

            // Destroy enemy after a short delay
            Destroy(gameObject, .5f);
        }
    }

    private void OnDestroy()
    {
        // Remove enemy from spawned enemies list when destroyed
        if (enemySpawn != null)
        {
            enemySpawn.spawnedEnemies.Remove(gameObject);
        }
    }
}
