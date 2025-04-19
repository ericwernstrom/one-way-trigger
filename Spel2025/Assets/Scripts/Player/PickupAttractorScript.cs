using UnityEngine;

public class PickupAttractor : MonoBehaviour
{
    [SerializeField]
    private Transform player; // Assign in Inspector or set in Start()

    private void Start()
    {
        // auto-find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            XPOrb orb = other.GetComponent<XPOrb>();
            if (orb != null)
            {
                orb.StartAttraction(player);
            }
        }
    }
}
