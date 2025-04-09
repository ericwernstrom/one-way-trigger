using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAttractorScript : MonoBehaviour
{
    public float attractionForce = 20f;
    private List<Rigidbody> attractedPickups = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && !attractedPickups.Contains(rb))
                attractedPickups.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
                attractedPickups.Remove(rb);
        }
    }

    private void FixedUpdate()
    {
        for (int i = attractedPickups.Count - 1; i >= 0; i--)
        {
            Rigidbody pickupRb = attractedPickups[i];
            if (pickupRb == null)
            {
                attractedPickups.RemoveAt(i);
                continue;
            }

            Vector3 direction = (transform.position - pickupRb.position).normalized;
            pickupRb.AddForce(direction * attractionForce);
        }
    }
}
