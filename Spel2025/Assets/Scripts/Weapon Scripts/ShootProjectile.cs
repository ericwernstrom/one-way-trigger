using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> projectile_prefabs;
    [SerializeField]
    private float rate_of_fire;
    [SerializeField]
    private float size_of_projectile;
    [SerializeField]
    private float speed_of_projectile;
    [SerializeField]
    private float lifetime_of_projectile;
    [SerializeField]
    private float upward_force = 0f; // New variable for throwables
    private ThirdPersonCam thirdPersonCam;

    private float time_last_projectile = 0;
    private int currentProjectileIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

        GameObject cameraBrain = GameObject.Find("CinemachineBrain");
        thirdPersonCam = cameraBrain.GetComponent<ThirdPersonCam>();
        //deafault settings if settings are 0 in unity
        if (rate_of_fire == 0){
            rate_of_fire = 1.0f;
        }
        if (size_of_projectile == 0)
        {
            size_of_projectile = 1.0f;
        }
        if (speed_of_projectile == 0)
        {
            speed_of_projectile = 30.0f;
        }
        if (lifetime_of_projectile == 0)
        {
            lifetime_of_projectile = 10.0f;
        }

        time_last_projectile = rate_of_fire;

    }

    // Update is called once per frame
    void Update()
    {
        
        float fire = Input.GetAxis("Fire1");
        time_last_projectile += Time.deltaTime;

        // when fire is pressed and time since last projectile is over rate of fire, fire projectile
        if (fire == 1 && time_last_projectile > rate_of_fire)
        {

            time_last_projectile = 0;
            Fire_projectile();
        }

        // Update isInCombat based on fire input
        if(fire == 1)
        {
            thirdPersonCam.CombatStart();
        } else {
            thirdPersonCam.CombatEnd();
        }

        // Cycle through projectiles if right mouse button (or another button) is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            CycleProjectile();
        }

    }

    private void Fire_projectile()
    {

        thirdPersonCam.CombatStart();
        
        // Calculate direction towards the center of the screen
        Camera mainCamera = Camera.main;
        Vector3 direction = mainCamera.transform.forward;
        //Vector3 direction = (targetPoint - transform.position).normalized;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        Vector3 targetPoint;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, shoot in the forward direction of the camera
            targetPoint = ray.GetPoint(1000); // Some far away point
        }

        //Rotates the projectile
        Quaternion specificRotation = Quaternion.Euler(90, 0, 0);

        // Get the currently selected projectile prefab
        GameObject projectile_prefab = projectile_prefabs[currentProjectileIndex];

        GameObject projectileInstance = Instantiate(projectile_prefab, transform.position, Quaternion.LookRotation(direction) * specificRotation);
        projectileInstance.transform.localScale = new Vector3(1f * size_of_projectile, 1f * size_of_projectile, 1f * size_of_projectile);

        // Calculate the initial velocity
        Vector3 initialVelocity = direction * speed_of_projectile;

        // If upward_force is specified, apply it to create an arc
        if (upward_force > 0)
        {
            initialVelocity += Vector3.up * upward_force;
        }

        projectileInstance.GetComponent<Rigidbody>().velocity = initialVelocity;

        // Find the player's colliders and ignore collisions with the instantiated projectile
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();
            Collider projectileCollider = projectileInstance.GetComponent<Collider>();
            if (projectileCollider != null)
            {
                foreach (Collider playerCollider in playerColliders)
                {
                    Physics.IgnoreCollision(projectileCollider, playerCollider);
                }
            }
        }

        // Destroy projectile after lifetime
        Destroy(projectileInstance, lifetime_of_projectile);

        
    }

    private void CycleProjectile()
    {
        if (projectile_prefabs.Count > 1)
        {
            currentProjectileIndex = (currentProjectileIndex + 1) % projectile_prefabs.Count;
        }
    }

}
