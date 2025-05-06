using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{

    [SerializeField]
    private GameObject healthPickupPrefab;
    [SerializeField]
    private int numberOfPickups = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DropHealth()
    {
        if (!Application.isPlaying || healthPickupPrefab == null) return;

        for (int i = 0; i < numberOfPickups; i++)
        {
            GameObject pickup = Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);

            // Optional: add a little force to scatter it
            Rigidbody rb = pickup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 scatter = Random.insideUnitSphere * 0.5f;
                rb.AddForce(scatter, ForceMode.Impulse);
            }
        }
    }

}
