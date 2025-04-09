using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectionSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            XPOrb experience_orb = other.GetComponent<XPOrb>();
            if (experience_orb != null)
            {
                experience_orb.Collect();
            }
        }
    }
}
