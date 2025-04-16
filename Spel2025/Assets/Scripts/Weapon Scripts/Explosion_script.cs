using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_script : MonoBehaviour
{
    
    private float timer = 0.0f;
    [SerializeField]
    private float instantiationInterval; // Set this to the desired interval between instantiations
    [SerializeField]
    private float max_scale;
    [SerializeField]
    private float scale_modifier;
    [SerializeField]
    private float linger_time;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Increases the size of the explosion if the size is less than the max size
        if (timer > instantiationInterval)
        {
            if (gameObject.transform.localScale.x >= max_scale)
            {
                Destroy(gameObject, linger_time);
            } else
            {
                gameObject.transform.localScale += new Vector3(scale_modifier, scale_modifier, scale_modifier);
            }
            timer = 0.0f;

        }
            
    }

}
