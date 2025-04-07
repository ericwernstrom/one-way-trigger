using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_holster : MonoBehaviour
{
    public GameObject[] prefabs; // Array to hold the prefabs
    public GameObject holster; // The empty object to which the prefab will be attached

    private int currentIndex = 0;
    private GameObject currentPrefabInstance;

    void Start()
    {
        if (prefabs.Length > 0)
        {
            CycleToNextPrefab();
        }
    }

    void Update()
    {
        // Change the key to 'K' to cycle through prefabs
        if (Input.GetKeyDown(KeyCode.K))
        {
            CycleToNextPrefab();
        }
    }

    void CycleToNextPrefab()
    {
        // Destroy the current prefab instance if it exists
        if (currentPrefabInstance != null)
        {
            Destroy(currentPrefabInstance);
        }

        // Instantiate the next prefab in the list
        currentPrefabInstance = Instantiate(prefabs[currentIndex], holster.transform.position, holster.transform.rotation, holster.transform);

        //Shotgun rotation wrong
        /*
        if (prefabs[currentIndex].name == "Shotgun")
        {
            currentPrefabInstance.transform.rotation = Quaternion.Euler(0, 0, 0); // Or any desired rotation
        }
        */
        // Update the index to point to the next prefab
        currentIndex = (currentIndex + 1) % prefabs.Length;
    }
}
