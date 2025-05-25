using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons_Explosion_script : MonoBehaviour
{
    [SerializeField]
    private static float max_damage = 60f;
    [SerializeField]
    private static float min_damage = 40f;
    [SerializeField]
    private static float max_knockback = 20f;
    [SerializeField]
    private static float min_knockback = 10f;
    [SerializeField]
    private static int maxLevel = 5;
    [SerializeField]
    private static int currentLevel = 0;

    private float starttime;
    private float damage_scaling;
    private float knockback_scaling;
    // Start is called before the first frame update
    void Start()
    {
        starttime = Time.time;
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

    //explosion level
    public static int GetCurrentExplosionDamageLevel() => currentLevel;

    //Upgrade explosion damage and knockback
    public static void ExplosionLevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            min_damage += 100f;
            max_damage += 100f;
            min_knockback += 5f;
            max_knockback += 5f;
        }
    }
    //Reset explosion damage and knockback to default values
    public static void ResetUpgrade()
    {
        currentLevel = 0;
        min_damage = 40f;
        max_damage = 60f;
        min_knockback = 10f;
        max_knockback = 20f;
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
