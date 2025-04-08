using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : MonoBehaviour
{
    public float explosionDelay = 1.0f; // Time delay before explosion
    public GameObject explosionPrefab; // Prefab for explosion effect
    public GameObject explosion_hitbox;
    public float explosionScale = 1.0f; // Scale of the explosion area

    private bool hasExploded = false;
    private bool isStuck = false;
    private Rigidbody rb;
    private Vector3 originalScale;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        originalScale = transform.localScale;

        // Start timer for explosion
        Invoke("Explode", explosionDelay);

    }

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;

            // Explosion prefab
            Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

            // Apply damage to nearby objects or enemies
            ApplyExplosionDamage();

            // Destroy the grenade object
            Destroy(gameObject);
        }
    }

    void ApplyExplosionDamage()
    {
        GameObject hitbox = Instantiate(explosion_hitbox, transform.position, explosion_hitbox.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isStuck)
        {
            isStuck = true;

            /*
            //Borde fungera men blir konstigt med scalen p? bomben
            isStuck = true;

            // Store the original scale
            Vector3 originalScale = transform.localScale;

            // Set the parent
            transform.SetParent(collision.contacts[0].otherCollider.transform);

            if (rb != null)
            {
                rb.isKinematic = true;
            }
            // Reset the local scale to maintain the original size
            transform.localScale = originalScale;
            */

            if (collision.rigidbody)
            {
                // Add a HingeJoint component to the gameobject that has the script
                HingeJoint hingeJoint = gameObject.AddComponent<HingeJoint>();

                // Set the rigidbody that we collided with as the connected body the hinge connects to
                hingeJoint.connectedBody = collision.rigidbody;

                // Make the joint act like a fixed connection
                hingeJoint.useLimits = true;
                JointLimits limits = new JointLimits();
                limits.min = 0;
                limits.max = 0;
                hingeJoint.limits = limits;

            }
            else
            {
               
               rb.isKinematic = true;
               
            }
           
        }

    }
}
