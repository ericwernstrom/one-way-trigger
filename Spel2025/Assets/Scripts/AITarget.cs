using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AITarget : MonoBehaviour
{
    private Transform Target;
    public float AttackDistance;

    private NavMeshAgent m_Agent;
    private float m_Distance;

    //Stun variables
    private bool isStunned = false;
    private float stunTimer = 0f;
    [SerializeField]
    private float stunDuration = 2f;


    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        Target = GameObject.FindGameObjectWithTag("Player").transform;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(m_Agent.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            m_Agent.Warp(hit.position); // or transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("Spawn position is not on the NavMesh");
        }

        if (m_Agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.");
            return;
        }

        if (Target == null)
        {
            Debug.LogError("Target is not set. Please assign a target.");
            return;
        }
    }

    void Update()
    {
        //Stun from stun_hitbox prefab
        if (isStunned)
        {
            // Stop the agent while stunned
            m_Agent.isStopped = true;

            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                // Start moving again
                m_Agent.isStopped = false;
            }
            return;
        }

        m_Distance = Vector3.Distance(m_Agent.transform.position, Target.position);
        // If the distance is greater than the attack distance, move towards the target
        if (m_Distance > AttackDistance)
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(Target.position);
        }
        // If the distance is less than the attack distance, stop moving
        else
        {
            m_Agent.isStopped = true;
        }
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        // Optionally: Stop navmesh, animations, etc.
    }
}
