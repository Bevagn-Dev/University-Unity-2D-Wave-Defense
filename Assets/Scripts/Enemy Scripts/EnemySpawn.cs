using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Singleton instance
    public static EnemySpawn Instance { get; private set; }

    // List of enemy types
    [Header("Enemy Types")]
    public List<Enemy> enemies = new List<Enemy>();

    // Array of spawn locations
    [Header("Spawn Locations")]
    public Transform[] spawnLocation;

    [Space(20)]

    // Spawner options
    [Header("Spawner Options")]
    [SerializeField] private int initialEnemyCount = 5;
    [SerializeField] private int enemyIncrementPerWave = 2;
    [SerializeField] private int waveDuration = 2;

    // Current wave number
    public int currentWave;

    // Timers and indices
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    private int waveValue;
    private int spawnIndex;

    // Lists to store enemies to spawn and spawned enemies
    [HideInInspector]
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> spawnedEnemies = new List<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start enemy spawning coroutine
    public void StartEnemySpawning()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    // Coroutine to spawn enemies
    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            if (spawnTimer <= 0)
            {
                if (enemiesToSpawn.Count > 0)
                {
                    GameObject enemy = Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity);
                    enemiesToSpawn.RemoveAt(0);
                    spawnedEnemies.Add(enemy);
                    spawnTimer = spawnInterval;

                    if (spawnIndex + 1 <= spawnLocation.Length - 1)
                    {
                        spawnIndex++;
                    }
                    else
                    {
                        spawnIndex = 0;
                    }
                }
                else
                {
                    waveTimer = 0;
                }
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;
            }

            if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
            {
                currentWave++;
                GenerateWave();
            }

            yield return null;
        }
    }

    // Generate wave of enemies
    public void GenerateWave()
    {
        waveValue = initialEnemyCount + (currentWave - 1) * enemyIncrementPerWave;
        GenerateEnemies();

        // Calculate spawn interval based on wave duration and enemies to spawn
        if (enemiesToSpawn.Count > 0)
        {
            spawnInterval = waveDuration / enemiesToSpawn.Count;
        }
        else
        {
            spawnInterval = 0;
        }

        waveTimer = waveDuration;
    }

    // Generate enemies to spawn
    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        int totalCost = 0;

        int maxTotalCost = initialEnemyCount + (currentWave - 1) * enemyIncrementPerWave;

        // Randomly select enemies to spawn based on cost constraints
        while (totalCost < maxTotalCost && generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (totalCost + randEnemyCost <= maxTotalCost)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                totalCost += randEnemyCost;
            }
            else
            {
                break;
            }
        }

        // Update list of enemies to spawn
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    // Enemy name, prefab, and cost
    public string enemyName;
    public GameObject enemyPrefab;
    public int cost;
}
