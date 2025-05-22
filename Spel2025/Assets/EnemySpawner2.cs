using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public EnemyData data;
        [HideInInspector]
        public float currentWeight;
    }

    [SerializeField]
    public List<Transform> spawnPoints = new List<Transform>();

    public List<EnemyEntry> enemies;
    public float initialSpawnInterval = 3f;
    public float minSpawnInterval = 0.5f;
    public float spawnIntervalDecreaseRate = 0.05f;
    public float difficultyIncreaseInterval = 10f;

    private float currentSpawnInterval;
    private float spawnTimer;
    private float difficultyTimer;

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;

        foreach (var entry in enemies)
        {
            entry.currentWeight = entry.data.baseWeight;
            Debug.Log($"Initialized enemy: {entry.data.enemyPrefab.name}, weight: {entry.currentWeight}");
        }

        spawnTimer = currentSpawnInterval;
        difficultyTimer = difficultyIncreaseInterval;

        Debug.Log("EnemySpawner started");
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        difficultyTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = currentSpawnInterval;
        }

        if (difficultyTimer <= 0f)
        {
            IncreaseDifficulty();
            difficultyTimer = difficultyIncreaseInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Calculate random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomIndex];

        EnemyEntry selected = GetRandomEnemyWeighted();

        Instantiate(selected.data.enemyPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"Spawned enemy: {selected.data.enemyPrefab.name} at {spawnPoint.position}");
    }

    private EnemyEntry GetRandomEnemyWeighted()
    {
        float totalWeight = 0f;
        foreach (var entry in enemies)
        {
            totalWeight += entry.currentWeight;
        }

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var entry in enemies)
        {
            cumulative += entry.currentWeight;
            if (randomValue <= cumulative)
            {
                Debug.Log($"Selected enemy: {entry.data.enemyPrefab.name} (randomValue: {randomValue}, cumulative: {cumulative})");
                return entry;
            }
        }

        Debug.LogWarning("Fallback enemy selected due to rounding issues");
        return enemies[enemies.Count - 1];
    }

    private void IncreaseDifficulty()
    {
        // Decrease spawn interval to increase spawn rate
        float oldInterval = currentSpawnInterval;
        currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecreaseRate);
        Debug.Log($"Increased difficulty: spawn interval decreased from {oldInterval} to {currentSpawnInterval}");

        // Increase weights for tougher enemies
        foreach (var entry in enemies)
        {
            float oldWeight = entry.currentWeight;
            entry.currentWeight += entry.data.weightIncreaseRate;
            Debug.Log($"Updated weight for {entry.data.enemyPrefab.name}: {oldWeight} → {entry.currentWeight}");
        }
    }
}
