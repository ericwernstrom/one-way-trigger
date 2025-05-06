using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO:
 * Spawn enemies after a certain amount of time
*/
public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    [SerializeField]
    private float spawnInterval = 5f;
    [SerializeField]
    private float spawnDistance = 10f;
    [SerializeField]
    private float spawnHeight = 1;
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Spawn every spawnInterval seconds
            yield return new WaitForSeconds(spawnInterval);

            // Generate new random position to spawn 
            Vector3 spawnOffset = new Vector3(Random.Range(- spawnDistance, spawnDistance + 1), spawnHeight, Random.Range( - spawnDistance, spawnDistance + 1));
            Instantiate(EnemyPrefab, transform.position + spawnOffset, Quaternion.identity);
        }
                
    }
}
