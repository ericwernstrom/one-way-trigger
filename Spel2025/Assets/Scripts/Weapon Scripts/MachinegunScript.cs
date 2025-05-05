using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunScript : MonoBehaviour
{
    public bool isFiring = false;

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
    private float decreaseDelay = 0.05f;	
    private float lastFired;
    private ThirdPersonCam thirdPersonCam;
    float maxRange = 1000f;
    Ray ray;
    RaycastHit hit;
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
        if  (delay > 0.05f)
        {
            delay -= decreaseDelay;
        }
        isFiring = true;
        thirdPersonCam.CombatStart();
    }

    public void StopFiring()
    {
        delay = 0.5f;
        isFiring = false;
        thirdPersonCam.CombatEnd();
    }


    void Update()
    {
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
