using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : MonoBehaviour
{
    private bool isFiring = false;

    // Track/Create bullet and hit point
    [Header("Raycast")]
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private ParticleSystem hitEffect;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    private float delay;
    private float lastFired;
    private ThirdPersonCam thirdPersonCam;
    float maxRange = 1000f;
    Ray ray;
    RaycastHit hit;
    [SerializeField]
    private float damage = 30f;
    // Layer mask to ignore certain layers (mainly the player layer)
    private LayerMask ignoreLayerMask;

    void Start()
    {
        ignoreLayerMask = LayerMask.GetMask("Ignore Raycast");
        GameObject cameraBrain = GameObject.Find("CinemachineBrain");
        thirdPersonCam = cameraBrain.GetComponent<ThirdPersonCam>();
    }
    public void StartFiring()
    {
        isFiring = true;
        thirdPersonCam.CombatStart();
    }

    public void StopFiring()
    {
        isFiring = false;
        thirdPersonCam.CombatEnd();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            thirdPersonCam.AimStart();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            thirdPersonCam.AimEnd();
        }
        // If it is time to fire shoot
        if (Time.time > lastFired + delay)
        {
            if (Input.GetMouseButton(0)) {
                lastFired = Time.time;
                StartFiring();
                Shoot();
            }
            else {
                StopFiring();
            }
        }
    }

    public void Shoot()
    {
        // Play muzzle flash
        muzzleFlash.Play();
        
        // Create ray from firepoint to crosshair to simulate aiming
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxRange, ~ignoreLayerMask))
        {
            // Create tracer if ray hits something
            TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));

            // Finds the hit objects health
            Health health = hit.collider.gameObject.GetComponent<Health>();
            if(health != null)
            {
                // Apply damage using the bullet's damage value
                health.TakeDamage(damage);
            }
        }
    }

    // Logic for creating a tracer
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / 0.1f;
            yield return null;// Makes it frame dependant
        }
        trail.transform.position = hit.point;
        // Instantiate hit effect
        ParticleSystem instantiatedHitEffect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        instantiatedHitEffect.Play();

        // Destroy the trail and hit effect after their duration
        Destroy(trail.gameObject, trail.time);
        Destroy(instantiatedHitEffect.gameObject, instantiatedHitEffect.main.duration);
    }
}
