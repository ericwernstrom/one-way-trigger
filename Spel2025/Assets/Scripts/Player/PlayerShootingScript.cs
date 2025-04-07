using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingScript : MonoBehaviour
{
    private ARShootingScript AssaultRifle;

    void Start()
    {
        AssaultRifle = GetComponentInChildren<ARShootingScript>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Check which gun is equipped
            AssaultRifle.StartFiring();
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Check which gun is equipped
            AssaultRifle.StopFiring();
        }
    }
}
