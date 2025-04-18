using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingRocketScript : MonoBehaviour
{
    private GameObject target;
    public float speed = 1f;
    public float trackingDelay = 2f; // Time in seconds before the rocket starts tracking the target
    private bool isTracking = false;
    private Rigidbody rb;
    public float turnSpeed = 1f; // Controls how quickly the rocket can turn

    //Explosion variables
    public GameObject explosion_prefab;
    public GameObject explosion_hitbox;
    public float explosionScale;

    //Stun variables (maybe remove if these are supposed to be projectiles)
    //Stun variables
    private bool isStunned = false;
    private float stunTimer = 0f;
    public float stunDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        if(target != null)
        {
            StartCoroutine(StartTrackingAfterDelay());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Stun from stun_hitbox prefab
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                // Resume behavior
            }
            return;
        }

        if (isTracking)
        {
            TrackTarget();
        }
    }

    private IEnumerator StartTrackingAfterDelay()
    {
        // Wait for the specified delay duration
        yield return new WaitForSeconds(trackingDelay);

        //Removes the initial upward velocity
        //Byta ut här så att det blir mer smooth kanske
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        // Start tracking the target
        isTracking = true;
    }

    private void TrackTarget()
    {
        if (target != null)
        {
            // Calculate direction towards the target
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Rotate the rocket to face the direction of movement gradually
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

            // Move the rocket forward in its current direction
            transform.position += transform.forward * speed * Time.deltaTime;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Spawns a hitbox upon collision
        GameObject hitbox = (GameObject)Instantiate(explosion_hitbox, transform.position, explosion_prefab.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        GameObject explosion = (GameObject)Instantiate(explosion_prefab, transform.position, explosion_prefab.transform.rotation);

        // Destroy the rocket upon collision
        Destroy(gameObject);
    }

    //Stun function
    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        // Optionally: Stop navmesh, animations, etc.
    }


}
