using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_hitbox_script : MonoBehaviour
{
    [SerializeField]
    private float max_damage;
    [SerializeField]
    private float min_damage;
    [SerializeField]
    private float max_knockback;
    [SerializeField]
    private float min_knockback;

    private float starttime;
    private float damage_scaling;
    private float knockback_scaling;
    // Start is called before the first frame update
    void Start()
    {
        starttime = Time.time;
    }

    public void Setup(float minDamage, float maxDamage, float minKnockback, float maxKnockback)
    {
        this.min_damage = minDamage;
        this.max_damage = maxDamage;
        this.min_knockback = minKnockback;
        this.max_knockback = maxKnockback;

        damage_scaling = ((max_damage - min_damage) / transform.localScale.x);
        knockback_scaling = ((max_knockback - min_knockback) / transform.localScale.x);
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy after time of spawn if nothing hits the hitbox
        if ((Time.time - starttime) > 0.2f) 
            Destroy(gameObject);

    }

    private void OnTriggerStay(Collider other)
    {

        
        if (other.attachedRigidbody != null)
        {
            // Make objects hit by the hitbox be blown back in the direction out of the explosion
            Vector3 dir = (other.attachedRigidbody.position - transform.position).normalized;
            float mag = (other.attachedRigidbody.position - transform.position).magnitude;
            float knockback = max_knockback - knockback_scaling * mag;
            if (knockback < min_knockback) knockback = min_knockback;
            other.attachedRigidbody.AddForce(dir * knockback);

            // Apply damage using the bullet's damage value
            float damage = max_damage - damage_scaling * mag;
            if (damage < min_damage) damage = min_damage;
            int roundedDamage = Mathf.RoundToInt(damage);

            // Damage enemies
            Health enemyHealth = other.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {

                enemyHealth.TakeDamage(roundedDamage);
            }

            // Damage player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(roundedDamage);
            }


        }


        Destroy(gameObject);

    }
}
