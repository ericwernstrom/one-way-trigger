using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage = 20; //Kanske kan vara i ett annat script men kan nog vara här

    //Stun variables
    private bool isStunned = false;
    private float stunTimer = 0f;
    [SerializeField]
    private float stunDuration = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
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

        if (target == null)
        {
            // Player gone/dead, stop chasing
            return;
        }

        // Move only in XZ plane
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Rotate only in XZ plane
        Vector3 direction = target.position - transform.position;
        Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);
        if (lookDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }


    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        // Optionally: Stop navmesh, animations, etc.
    }
}
