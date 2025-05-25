using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillHurtbox_script : MonoBehaviour
{
    private int damage = 9999;
    // Start is called before the first frame update


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

}
