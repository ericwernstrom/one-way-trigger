using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_hitbox_script : MonoBehaviour
{
    [SerializeField]
    private float stunDuration = 2f;

    void Start()
    {
        // You can destroy this hitbox shortly after it triggers
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Checking for EnemyFollowScript
            AITarget enemyAI = other.GetComponent<AITarget>();
            if (enemyAI != null)
            {
                enemyAI.Stun(stunDuration);
            }

            // Check for TrackingRocketScript
            TrackingRocketScript rocket = other.GetComponent<TrackingRocketScript>();
            if (rocket != null)
            {
                rocket.Stun(stunDuration);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
