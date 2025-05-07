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
    public int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    [SerializeField]
    public List<Transform> spawnPoints = new List<Transform>();

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;



    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
        else
        {
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = enemiesToSpawn[0];
                enemiesToSpawn.RemoveAt(0);
                // Calculate random spawn point
                int randomIndex = Random.Range(0, enemies.Count);
                Transform spawnPoint = spawnPoints[randomIndex];
                // Spawn enemy
                Instantiate(enemy, spawnPoint.position, Quaternion.identity);
                // Reset spawn timer
                spawnTimer = spawnInterval;
            }
        }
    }

    public void GenerateWave()
    {
        waveValue = currentWave * 10;   // Higher wave value for more enemies
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;    // The time between spawning each enemy
        waveTimer = waveDuration; // The time until the wave ends
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while(waveValue > 0)
        {
            // Randomly select an enemy from the list
            int randomIndex = Random.Range(0, enemies.Count);
            Enemy enemy = enemies[randomIndex];

            // Check if the enemy can be spawned with the current wave value
            if (waveValue - enemy.cost >= 0)
            {
                waveValue -= enemy.cost;
                generatedEnemies.Add(enemy.enemyPrefab);
            }
            else
            {
                break;
                // Todo: If the enemy cannot be spawned, remove it from the list instead of break?
                //enemies.RemoveAt(randomIndex);
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}
