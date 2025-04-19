using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private float explosionDelay = 1.6f; // Time delay before explosion
    [SerializeField]
    private GameObject explosionPrefab; // Prefab for explosion effect
    [SerializeField]
    private GameObject explosion_hitbox;
    [SerializeField]
    private float explosionScale = 1.0f; //Scale of the explosion area

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
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * explosionScale;

            Destroy(explosion, 1.5f);

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
