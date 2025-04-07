using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShootingScript : MonoBehaviour
{
    public bool isFiring = false;

    // Track/Create bullet and hit point
    public Transform firePoint;
    public ParticleSystem hitEffect;
    public ParticleSystem muzzleFlash;
    public TrailRenderer bulletTrail;
    float maxRange = 1000f;
    private ThirdPersonCam thirdPersonCam;
    Ray ray;
    RaycastHit hit;
    public float damage = 5f;
    // Layer mask to ignore certain layers (mainly the player layer)
    private LayerMask ignoreLayerMask;

    void Start(){
        ignoreLayerMask = LayerMask.GetMask("Ignore Raycast");
        GameObject cameraBrain = GameObject.Find("CinemachineBrain");
        thirdPersonCam = cameraBrain.GetComponent<ThirdPersonCam>();
    }
    public void StartFiring()
    {
        float totalDamage = 0f;
        Health targetHealth = null;

        for (int i = 0; i < 8; i++)
        {
            Vector3 bulletDirection = randomizeDirection();

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(firePoint.position, bulletDirection, out hit, maxRange, ~ignoreLayerMask))
            {
                //bullettrace
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));

                // Finds the hit objects health
                Health health = hit.collider.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    totalDamage += damage;
                    targetHealth = health;
                }

            }
        }

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(totalDamage);
        }

        thirdPersonCam.isInCombat = true;
        isFiring = true;
        muzzleFlash.Play();
    }
    public void StopFiring()
    {
        isFiring = false;
        thirdPersonCam.isInCombat = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartFiring();    
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopFiring();
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / 0.1f;
            yield return null;
        }
        trail.transform.position = hit.point;
        ParticleSystem instantiatedHitEffect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        instantiatedHitEffect.Play();

        Destroy(trail.gameObject, trail.time);
    }

    private Vector3 randomizeDirection() {
        // Calculate direction towards the center of the screen
        Camera mainCamera = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        Vector3 direction = ray.direction;

        // Randomize the direction slightly
        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);
        float z = Random.Range(-0.1f, 0.1f);

        Vector3 randomizedDirection = direction + new Vector3(x, y, z);
        return randomizedDirection.normalized;
        /*
        //Old: Shoot forward from the barrel
        // Randomize the direction of the bullet
        Vector3 generalDirection = transform.forward;
        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);
        float z = Random.Range(-0.1f, 0.1f);

        Vector3 bulletDirection = generalDirection + new Vector3(x,y,z);

        return bulletDirection;
        */
    }
}
