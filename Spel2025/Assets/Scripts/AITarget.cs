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

    
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        Transform Target = GameObject.FindGameObjectWithTag("Player").transform;

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
        Transform Target = GameObject.FindGameObjectWithTag("Player").transform;

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
}
