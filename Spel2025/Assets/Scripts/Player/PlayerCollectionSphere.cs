using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectionSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            XPOrb orb = other.GetComponent<XPOrb>();
            if (orb != null)
            {
                orb.Collect();
            }
        }
    }
}
