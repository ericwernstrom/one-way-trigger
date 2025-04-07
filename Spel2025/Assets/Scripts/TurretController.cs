using UnityEngine;

// Controls player movement and rotation.
public class TurretController : MonoBehaviour
{
    [Header("Rotation & Detection")]
    public float rotationSpeed = 1.0f; // Set turret's rotation speed.
    public Transform target; // target to detect and follow
    public float seeRange = 12.0f; //maximum attack distance – will attack if closer than this to the enemy
    public float sightAngle = 60f; // field of view in degrees

    [Header("Shooting")]
    public float fireRate; // Number of projectiles per second
    public Transform firePoint; // The point from which the projectile will be fired
    public GameObject projectilePrefab; // The projectile that will be spawned

    private float nextFireTime;


    // Start is called before the first frame update
    private void Start()
    {
        nextFireTime = Time.time + (1f / fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeeTarget())
        {
            Rotate();
            Shoot();
        }
        else
        {
            Survey();
        }

        
    }

    bool CanSeeTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);
        // Can not see target if outside of range or field of view
        if (Vector3.Distance(transform.position, target.position) > seeRange || angle > sightAngle)
            return false;

        return true;
    }

    void Rotate()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        // Rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.LookRotation(direction),
        rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void Survey()
    {
        
    }
}