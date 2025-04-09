using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrackingRocket : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float initialUpwardSpeed = 5f; // Adjust this value as needed
    public float spawnInterval = 2f; // Time between spawns



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRocketRoutine());
    }

    IEnumerator SpawnRocketRoutine()
    {
        while (true)
        {
            SpawnRocket();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.Euler(270, 180, 0));

        Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();
        if (rocketRb != null)
        {
            rocketRb.velocity = transform.up * initialUpwardSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
