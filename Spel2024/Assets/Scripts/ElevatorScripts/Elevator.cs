using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    // Reference to the parent which has all of the movement logic
    public GameObject elevator;
    private SimpleTranslator otherScript;

    private void Start()
    {
        otherScript = elevator.GetComponent<SimpleTranslator>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            otherScript.activate = true;
            Debug.Log("trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //elevator.activate = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
