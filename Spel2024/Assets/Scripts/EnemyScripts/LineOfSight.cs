
using System.Collections;


using System.Collections.Generic;
using UnityEngine;


public class LineOfSight : MonoBehaviour
{
    // Detection and movement
    public Transform target; // target to detect and follow
    public Transform head;
    public float seeRange = 12.0f; //maximum attack distance – will attack if closer than this to the enemy
    float shootRange = 8.0f;
    public float keepDistance = 2.0f; //closest distance to get to enemy
    public float rotationSpeed = 4.0f;
    public float speed = 0.1f;
    public float sightAngle = 60f; // field of view
    private Rigidbody enemyRb;
    // Jumping
    private float lastJumpTime;
    public float jumpInterval = 3f; // Seconds between jump


    // Methods
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        lastJumpTime = Time.time; // Intilialize time of last jump
    }

    void Update()
    {
        if (CanSeeTarget())
        {
            if (CanShoot())
                Shoot();
            else
                Pursue();
        }
        if (CanJump())
            Jump();
        else
        {
            //stand around
            /*
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            */
        }
    }

    bool CanSeeTarget()
    {
        Vector3 directionToTarget = target.position - head.position;
        float angle = Vector3.Angle(directionToTarget, head.forward);
        // Can not see target if outside of range or field of view
        if (Vector3.Distance(head.position, target.position) > seeRange || angle > sightAngle)
            return false;

        return true;
    }

    bool CanShoot()
    {
        if (Vector3.Distance(head.position, target.position) > shootRange)
            return false;

        return true;
    }

    void Pursue()
    {
        Vector3 position = target.position;
        Vector3 direction = position - head.position;
        direction.y = 0;

        // Rotate towards the target
        head.rotation = Quaternion.Slerp(head.rotation,
        Quaternion.LookRotation(direction),
        rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // Move the character
        if (direction.magnitude > keepDistance)
        {
            direction = direction.normalized * speed;
            transform.position += direction;
            //enemyRb.AddForce(direction);
        }
    }

    void Shoot()
    {
        Vector3 position = target.position;
        Vector3 direction = position - head.position;
        direction.y = 0;

        // Rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    bool CanJump()
    {
        if (Time.time - lastJumpTime > jumpInterval)
            return true;
        else
            return false;
    }
    void Jump()
    {
        Debug.Log("Jump!");
        enemyRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        lastJumpTime = Time.time;
    }
}

