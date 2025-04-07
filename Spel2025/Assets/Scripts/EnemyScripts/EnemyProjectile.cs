using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int lifetime = 5;

    void Start(){
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collides with something
        // For example, you can destroy the projectile when it hits an enemy
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the projectile
            // You may want to add more behavior here, such as dealing damage to the enemy
        }
    }
}
