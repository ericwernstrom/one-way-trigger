using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_holster : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs; // Array to hold the prefabs
    [SerializeField]
    private GameObject holster; // The empty object to which the prefab will be attached

    private int currentIndex = 0;
    private GameObject currentPrefabInstance;

    void Start()
    {
        if (prefabs.Length > 0)
        {
            EquipPrefab(currentIndex);
        }
    }

    void Update()
    {
        // Change weapon with number keys
        for (int i = 0; i < Mathf.Min(9, prefabs.Length); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipPrefab(i);
                return;
            }
        }

        // Mouse scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            CycleNext();
        }
        else if (scroll < 0f)
        {
            CyclePrevious();
        }
    }

    void EquipPrefab(int index)
    {
        if (index < 0 || index >= prefabs.Length) return;

        if (currentPrefabInstance != null)
        {
            Destroy(currentPrefabInstance);
        }

        currentIndex = index;
        currentPrefabInstance = Instantiate(prefabs[currentIndex], holster.transform.position, holster.transform.rotation, holster.transform);
    }

    void CycleNext()
    {
        int nextIndex = (currentIndex + 1) % prefabs.Length;
        EquipPrefab(nextIndex);
    }

    void CyclePrevious()
    {
        int prevIndex = (currentIndex - 1 + prefabs.Length) % prefabs.Length;
        EquipPrefab(prevIndex);
    }
}
