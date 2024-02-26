using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour, IDamageable
{
    [Header("Health Options")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private EnemyHealthBar enemyHealthBar;

    public UnityEvent enemyDeath;

    private int currentHealth;

    private void Awake() 
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();

        enemyDeath.AddListener(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().AddMoney);

        currentHealth = maxHealth;   
        enemyHealthBar.SetMaxHealth(maxHealth);     
    }

    private void Update()
    {
        enemyHealthBar.setHealth(currentHealth);
    }

    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            enemyDeath.Invoke();
            Destroy(gameObject, .5f);
        }
    }

    private void OnDestroy() 
    {
        if (GameObject.FindGameObjectWithTag("EnemySpawner") != null)
        {
            GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawn>().spawnedEnemies.Remove(gameObject);
        }
    }

}
