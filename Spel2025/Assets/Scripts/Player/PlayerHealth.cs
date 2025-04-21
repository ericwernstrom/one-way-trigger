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
    private HealthBar HealthBar;
    [SerializeField]
    private GameManagerScript gameManager;

    private bool isDead;

    // Method to decrease health
    public void TakeDamage(int damage)
    {
        health -= damage;
        //HealthBar.SetHealth(health); // Update health bar

        // Dead if health is less than or equal to zero
        if (health <= 0 && !isDead)
        {
            isDead = true;

            //Disable player when dead
            gameObject.SetActive(false);

            //Call the gameOver function from gameManager which starts the gameoverscreen
            gameManager.gameOver();

            Debug.Log("Dead");
        }
    }

    void Respawn()
    {
        HealthBar.SetMaxHealth(100); // Reset health
    }
}

