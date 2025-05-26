using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Gives player gameobject a health attribute that decreases when hit by
/// tagged projectiles. The other (projectile) gameObject must have one of the matching tags
/// below (e.g. Rocket), as well as a Collider component with Is Trigger set to ¨
/// True to work. Depending on the projectile tag, different amounts of damage is
/// received.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    private int health = 100; // Player's starting health
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameManagerScript gameManager;
    private int maxHealth = 100; // Players max possible health
    private int currentHealthLevel = 0;
    private int maxLevel = 5; // Max level for health upgrades

    private bool isDead;

    //HUD UI
    [SerializeField]
    TextMeshProUGUI hpAmountText;

    // AUDIO
    [SerializeField]
    private AudioClip death_sound;
    private AudioClip hit_sound;
    private AudioSource audioSource;

    public void Start()
    {
        // AUDIO
        audioSource = GetComponent<AudioSource>();
        hit_sound = audioSource.clip;
    }

    //Get Current level of health
    public int CurrentHealthLevel() => currentHealthLevel;


    // Method to decrease health
    public void TakeDamage(int damage)
    {
        health -= damage;
        //Change the healthBar
        healthBar.value = health;
        //Change the health text
        hpAmountText.text = health.ToString() + "/" + maxHealth.ToString();

        // AUDIO 
        audioSource.PlayOneShot(hit_sound);
        // Dead if health is less than or equal to zero
        if (health <= 0 && !isDead)
        {
            isDead = true;
            // AUDIO
            AudioSource.PlayClipAtPoint(death_sound, transform.position);

            //Disable player when dead
            //gameObject.SetActive(false);

            //Call the gameOver function from gameManager which starts the gameoverscreen
            gameManager.gameOver();

            //Debug.Log("Dead");
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        healthBar.value = health;
        //Update hpAmount text
        hpAmountText.text = health.ToString() + "/" + maxHealth.ToString();

        if (health >= maxHealth)
        {
            health = maxHealth;
            healthBar.value = health;
            hpAmountText.text = health.ToString() + "/" + maxHealth.ToString();
        }
    }
    public void IncreaseMaxHealth()
    {
        if (currentHealthLevel < maxLevel)
        {
            currentHealthLevel++;
            maxHealth += 20;
            //Update healthbar
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
            //Update hpamount text
            hpAmountText.text = health.ToString() + "/" + maxHealth.ToString();
        }
        else
        {
            Debug.Log("Max health level reached");
        }
    }


    void Respawn()
    {
        //HealthBar.SetMaxHealth(100); // Reset health
    }
}

