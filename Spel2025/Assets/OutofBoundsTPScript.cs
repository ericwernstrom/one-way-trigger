using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBoundsTPScript : MonoBehaviour
{
    private Transform tpPoint;
    // Start is called before the first frame update
    void Start()
    {
        // Find the child named "TPPoint"
        tpPoint = transform.Find("TPPoint");
        if (tpPoint == null)
        {
            Debug.LogError("No child named TPPoint found!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = tpPoint.position;
        }
    }
}
