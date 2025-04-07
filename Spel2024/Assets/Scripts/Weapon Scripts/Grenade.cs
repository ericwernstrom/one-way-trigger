using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 2f; // Time delay before explosion
    public GameObject explosionPrefab; // Prefab for explosion effect
    public GameObject explosion_hitbox;
    public float explosionScale = 1.0f; //Scale of the explosion area

    private bool hasExploded = false;

    void Start()
    {
        // Start countdown for explosion
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;

            // Instantiate explosion effect
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Apply damage to nearby objects or enemies
            ApplyExplosionDamage();


            // Destroy the grenade object
            Destroy(gameObject);
        }
    }

    
    void ApplyExplosionDamage()
    {
        GameObject hitbox = (GameObject)Instantiate(explosion_hitbox, transform.position, explosion_hitbox.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);
    }
    
}
