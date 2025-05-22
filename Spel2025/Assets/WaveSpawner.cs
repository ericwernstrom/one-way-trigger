using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}

public class WaveSpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int currentWave;
    public int waveBudget;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    [SerializeField]
    public List<Transform> spawnPoints = new List<Transform>();

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    
    // Generate first wave and increase wave number (doesn't spawn enemies)
    void Start()
    {
        if (waveTimer <= 0 || enemiesToSpawn.Count == 0)
        {
            currentWave++;
            GenerateWave();
            Debug.Log($"[START] Wave {currentWave} started! Enemies to spawn: {enemiesToSpawn.Count}");
        }
    }

    // Spawns enemies of the current wave at regular intervals. New wave is generated when the current one is completed (wave duration or all enemies killed).
    void FixedUpdate()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
        else if (enemiesToSpawn.Count > 0)
        {
            GameObject enemy = enemiesToSpawn[0];
            enemiesToSpawn.RemoveAt(0);

            // Calculate random spawn point
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Spawn enemy
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);

            // Reset spawn timer
            spawnTimer = spawnInterval;
        }

        // Is it time to generate a new wave?
        if (waveTimer <= 0 || enemiesToSpawn.Count == 0)
        {
            currentWave++;
            GenerateWave();
           // Debug.Log($"[NEXT WAVE] Wave {currentWave} started! Enemies to spawn: {enemiesToSpawn.Count}");
            //Debug.Log($"Spawn INterval: {spawnInterval} seconds.");
        }
    }

    public void GenerateWave()
    {
        // Calculate the budget for the wave
        waveBudget = Mathf.FloorToInt(Mathf.Pow(1.25f, currentWave) * 10f);

        GenerateEnemies();

        // Adjust spawn interval dynamically
        float minSpawnInterval = 0.5f;
        float maxSpawnInterval = 3f;
        float difficulty = Mathf.Clamp01(currentWave / 20f);
        spawnInterval = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, difficulty);

        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        int attempts = 0;

        while (waveBudget > 0 && attempts < 1000) // safety limit
        {
            int randomIndex = Random.Range(0, enemies.Count);
            Enemy enemy = enemies[randomIndex];

            if (waveBudget - enemy.cost >= 0)
            {
                waveBudget -= enemy.cost;
                generatedEnemies.Add(enemy.enemyPrefab);
                //Debug.Log($"[ENEMY SELECTED] Added {enemy.enemyPrefab.name} (cost: {enemy.cost}) | Remaining value: {waveBudget}");
            }

            attempts++;
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}
