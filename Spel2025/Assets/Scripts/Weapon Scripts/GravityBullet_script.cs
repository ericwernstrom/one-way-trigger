using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityBullet_script : MonoBehaviour
{
    readonly float G = 40f;
    GameObject[] enemies;

    [SerializeField]
    private GameObject hit_particles;

    [SerializeField]
    private float damage = 20f;
    // Start is called before the first frame update
    void Start()
    {
        //Kan nog tas bort
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        //Gravity();
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the object within the trigger is an enemy
        if (other.CompareTag("Enemy"))
        {
            Gravity(other.gameObject);
        }
    }

    void Gravity(GameObject other_enemy)
    {
        if (other_enemy != null && gameObject != other_enemy)
        {

            float r = Vector3.Distance(gameObject.transform.position, other_enemy.transform.position);

            float m1 = gameObject.GetComponent<Rigidbody>().mass;
            float m2 = other_enemy.GetComponent<Rigidbody>().mass;


            gameObject.GetComponent<Rigidbody>().AddForce(G * ((m1 * m2) / (r * r)) * (other_enemy.transform.position - gameObject.transform.position).normalized);
        }
        /*
        foreach (GameObject other_enemy in enemies)
        {

            if (other_enemy != null && gameObject != other_enemy)
            {

                float r = Vector3.Distance(gameObject.transform.position, other_enemy.transform.position);
                
                float m1 = gameObject.GetComponent<Rigidbody>().mass;
                float m2 = other_enemy.GetComponent<Rigidbody>().mass;


                gameObject.GetComponent<Rigidbody>().AddForce(G * ((m1 * m2) / (r * r)) * (other_enemy.transform.position - gameObject.transform.position).normalized);
            }
        }
        */

    }

    private void OnCollisionEnter(Collision collision)
    {

        // Get the health component from the collided object
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            // Apply damage using the bullet's damage value
            health.TakeDamage(damage);
        }

        // Spawns Particles
        GameObject particles = (GameObject)Instantiate(hit_particles, transform.position, hit_particles.transform.rotation);
        // Destroy the bullet
        Destroy(gameObject);

    }

}
