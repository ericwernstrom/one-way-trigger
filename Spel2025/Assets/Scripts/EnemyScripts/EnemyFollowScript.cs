using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage = 20f; //Kanske kan vara i ett annat script men kan nog vara här

    //Stun variables
    private bool isStunned = false;
    private float stunTimer = 0f;
    [SerializeField]
    private float stunDuration = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stun from stun_hitbox prefab
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                // Resume behavior
            }
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Make the enemy look at the player
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        // Optionally: Stop navmesh, animations, etc.
    }
}
