using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Gives player gameobject a health attribute that decreases when hit by
/// tagged projectiles. The other (projectile) gameObject must have one of the matching tags
/// below (e.g. Rocket), as well as a Collider component with Is Trigger set to ¨
/// True to work. Depending on the projectile tag, different amounts of damage is
/// received.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 100; // Enemy's starting health
    [SerializeField]
    private int rocket_damage = 10;
    [SerializeField]
    private HealthBar HealthBar;

    // This method is called when another collider enters the trigger collider
    
    /*
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a rocket
        if (other.gameObject.CompareTag("Projectile"))
        {
            // Decrease health
            TakeDamage(rocket_damage);

            // Destroy the bullet
            Destroy(other.gameObject);
        }
    }*/

    // Method to decrease health
    void TakeDamage(int damage)
    {
        health -= damage;
        HealthBar.SetHealth(health); // Update health bar

        // Dead if health is less than or equal to zero
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle enemy death
    void Die()
    {
        // You can add any death effects here like playing a sound or animation
        Destroy(transform.parent.gameObject); // Destroy the player object
        // Respawning can be implemented here
        Respawn();
    }

    void Respawn()
    {
        HealthBar.SetMaxHealth(100); // Reset health
    }
}

