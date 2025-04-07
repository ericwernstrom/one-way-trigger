using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBeam : MonoBehaviour
{
    public float initial_scale;
    private float current_scale;
    public float final_scale;
    public float speed;
    public float linger_time;
    private float timer;
    public float cirle_height;

    // Start is called before the first frame update
    void Start()
    {
        current_scale = initial_scale;
    }

    // Update is called once per frame
    void Update()
    {
        // Increases the size of the beam
        if (current_scale <= final_scale){
                    current_scale += speed * Time.deltaTime;
                    transform.localScale = new Vector3(current_scale, current_scale, cirle_height);
        // Makes the beam linger for a while once max size has been reached
        } else {
            if (timer < linger_time){
                timer += Time.deltaTime;
            } else {
                current_scale = initial_scale;
                timer = 0;
            }
        }
    }
}
