using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Gives enemy gameobjects a health attribute that decreases when hit by
/// tagged projectiles. The other gameObject must have one of the matching tags
/// below (e.g. Rocket), as well as a Collider component with Is Trigger set to ¨
/// True to work. Depending on the projectile tag, different amounts of damage is
/// received.
/// </summary>
public class Health : MonoBehaviour
{


    public float health = 100f; // Enemy's starting health
    private TextMeshPro PopupPrefab;


    // This method is called when another collider enters the trigger collider


    void Start()
    {
        PopupPrefab = Resources.Load<TextMeshPro>("DamagePopupTextbox");
    }


    // Method to decrease health
    public void TakeDamage(float damage)
    {
        health -= damage;

        // Dead if health is less than or equal to zero
        if (health <= 0)
        {
            Die();
        }

        TextMeshPro DamagePopup = Instantiate(PopupPrefab, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
        DamagePopup.text = "-" + damage;

    }

    // Method to handle enemy death
    void Die()
    {
        XPDropper dropper = GetComponent<XPDropper>();
        if (dropper != null)
        {
            dropper.DropXP();
        }
        // You can add any death effects here like playing a sound or animation
        Destroy(gameObject); // Destroy the enemy gameObject
    }
}

