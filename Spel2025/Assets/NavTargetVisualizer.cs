using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class NavTargetVisualizer : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnDrawGizmos()
    {
        Debug.Log("call");

        if (agent != null && agent.hasPath)
        {
            Debug.Log("Agent has path to destination: " + agent.destination);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, agent.destination);
            Gizmos.DrawSphere(agent.destination, 0.3f);
        }
    }
}
