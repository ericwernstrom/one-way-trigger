using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform firePoint; // The point from which the projectile will be fired
    [SerializeField]
    private float fireRate; // Number of projectiles per second

    private float nextFireTime;

    void Start()
    {
        // Initialize nextFireTime to prevent immediate firing
        nextFireTime = Time.time + (1f / fireRate);
    }

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void FireProjectile()
    {
        // Instantiate the projectile at the fire point position and rotation
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
