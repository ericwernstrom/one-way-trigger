using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : MonoBehaviour
{
    [SerializeField]
    private float explosionDelay = 1.0f; // Time delay before explosion
    [SerializeField]
    private GameObject explosionPrefab; // Prefab for explosion effect
    [SerializeField]
    private GameObject explosion_hitbox;
    [SerializeField]
    private float explosionScale = 1.0f; // Scale of the explosion area

    private bool hasExploded = false;
    private bool isStuck = false;
    private Rigidbody rb;
    private Vector3 originalScale;

    // AUDIO
    private AudioSource audioSource;
    [SerializeField] private AudioClip timer_sound;
    [SerializeField] private AudioClip throwing_sound;
    [SerializeField] private AudioClip sticking_sound;
    [SerializeField]  private AudioClip explostion_sound;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        originalScale = transform.localScale;

        // Start timer for explosion
        Invoke("Explode", explosionDelay);

        // AUDIO
        audioSource = GetComponent<AudioSource>();
        // todo: play throwing sfx

        // todo: Start playing timer sound
        //audioSource.Play();


    }

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;

            // Explosion prefab
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * explosionScale;

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

            // AUDIO: Play stuck sound
            audioSource.PlayOneShot(sticking_sound);
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
