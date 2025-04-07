using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyScript : MonoBehaviour
{
    public Transform walk_position_1;
    public Transform walk_position_2;
    public float speed;

    private Vector3 dir;


    // Start is called before the first frame update
    void Start()
    {
        dir = walk_position_2.position - walk_position_1.position;
        transform.position = walk_position_1.position + dir.normalized * 4;
    }

    private void OnTriggerEnter(Collider other)
    {
        speed = speed * -1;
    }
    // Update is called once per frame
    void Update()
    {      
        
        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
