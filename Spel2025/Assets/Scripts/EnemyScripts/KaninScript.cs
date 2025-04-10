using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaninScript : MonoBehaviour
{
    [Header("Behaviour & physics")]
    private bool Jumping = false;
    public float jump_power;
    public float AssCanon_firetime;
    public float shoot_power;
    public float extra_gravity;

    [Header("Rotation & Detection")]
    public float rotationSpeed = 1.0f; // Set turret's rotation speed.
    public float seeRange = 12.0f; //maximum attack distance – will attack if closer than this to the enemy
    public float sightAngle = 60f; // field of view in degrees

    [Header("Shooting")]
    public float fireRateAssCanon; // Number of projectiles per second
    public float fireRateMainWeapon;
    public Transform firePointAssCanon; // The point from which the projectile will be fired
    public Transform firePointMainWeapon;
    public GameObject projectilePrefab; // The projectile that will be spawned
    GameObject player;
    private float nextFireTimeAssCanon;
    private float nextFireTimeMainWeapon;

    private float time_of_jump;
    private float player_height = 0.49f;
    private float ray_distance = 0.1f;
    private Vector3 shoot_direction = Vector3.zero;
    Ray ray_down = new Ray();
    RaycastHit hit_down;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        time_of_jump = AssCanon_firetime;
        nextFireTimeAssCanon = Time.time + (1f / fireRateAssCanon);
        nextFireTimeMainWeapon = Time.time + (1f / fireRateMainWeapon);
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !Jumping)
        {
            shoot_direction = Vector3.down;
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.up, transform.position - other.gameObject.transform.position);
            Jump();
        }
    }


    private void OnTriggerExit(Collider other) {

        if (other.CompareTag("Player"))
        {
            shoot_direction = (other.gameObject.transform.position - transform.position).normalized;
            gameObject.transform.rotation = Quaternion.LookRotation(-shoot_direction, Vector3.up);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Jumping)
        {
            rb.AddForce(Vector3.down * extra_gravity * Time.deltaTime);
            if ((time_of_jump + AssCanon_firetime) > Time.time)
            {
                AssCanon();
            }
            else
            {
                gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), Vector3.up);
                Jumping = GroundCheck();
            }
        }
        else if(CanSeeTarget())
        {
            Rotate();
            ShootMainWeapon();
        }
            
  
    }
    private bool GroundCheck()
    {
        // lagg till collision ground check
        ray_down.origin = transform.position + new Vector3(0, -player_height, 0);
        ray_down.direction = new Vector3(0, -1f, 0);
        if (Physics.Raycast(ray_down, out hit_down, ray_distance))
        {

            return true;
        }
        else return false;
    }


    private void Jump()
    {
        Jumping = true;
        time_of_jump = Time.time;
        rb.AddForce(Vector3.up * jump_power);
    }

    private void AssCanon()
    { 
        if (Time.time > nextFireTimeAssCanon)
        {
            Instantiate(projectilePrefab, firePointAssCanon.position, firePointAssCanon.rotation);
            nextFireTimeAssCanon = Time.time + (1f / fireRateAssCanon);
            rb.AddForce(-shoot_direction * shoot_power);
        }
    }
    bool CanSeeTarget()
    {
        Vector3 directionToTarget = player.transform.position - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);
        // Can not see target if outside of range or field of view
        if (Vector3.Distance(transform.position, player.transform.position) > seeRange || angle > sightAngle)
            return false;

        return true;
    }
    void Rotate()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        // Rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.LookRotation(direction),
        rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    void ShootMainWeapon()
    {
        if (Time.time > nextFireTimeMainWeapon)
        {
            Vector3 direction = (player.transform.position - firePointMainWeapon.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction); // For 3D
            Instantiate(projectilePrefab, firePointMainWeapon.position, rotation);
            nextFireTimeMainWeapon = Time.time + (1f / fireRateMainWeapon);
            
        }
    }
}
