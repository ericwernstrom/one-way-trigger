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
    private float spawnInterval = 3f;
    [SerializeField]
    private float spawnArea = 10f;    
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(spawnInterval);
            // Generate new random position to spawn 
            Vector3 spawnOffset = new Vector3(Random.Range(-10, 11), 2, Random.Range(-10, 11));
            Instantiate(EnemyPrefab, transform.position + spawnOffset, Quaternion.identity);
        }
                
    }
}
