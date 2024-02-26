using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawn : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public int initialEnemyCount = 5;
    public int enemyIncrementPerWave = 2;

    public UnityEvent updateWaveCount;

    private void Awake() 
    {
        updateWaveCount.AddListener(GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>().IncreaseWaveCount);        
    }

    public void StartEnemySpawning()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

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
                updateWaveCount.Invoke();
                currWave++;
                GenerateWave();
            }

            yield return null;
        }
    }

    public void GenerateWave()
    {
        waveValue = initialEnemyCount + (currWave - 1) * enemyIncrementPerWave;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        int totalCost = 0;

        int maxTotalCost = initialEnemyCount + (currWave - 1) * enemyIncrementPerWave;

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
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
