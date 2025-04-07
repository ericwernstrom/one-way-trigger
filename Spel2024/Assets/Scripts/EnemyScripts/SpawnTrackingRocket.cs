using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrackingRocket : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float initialUpwardSpeed = 5f; // Adjust this value as needed
  


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //JUST NU: spawna projektil med E, ändra sen när enemies ska använda den.
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.Euler(270, 180, 0));
            
            // Set the initial upward velocity
            Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();
            if (rocketRb != null)
            {
                rocketRb.velocity = transform.up * initialUpwardSpeed;
            }
        }
    }
}
